using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Web.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace IdeaMachineWeb.Utils
{
	public class DestructuringModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder? GetBinder(ModelBinderProviderContext context)
		{
			return typeof(ITuple).IsAssignableFrom(context.Metadata.ModelType)
				? new DestructuringModelBinder()
				: null;
		}
	}

	public class DestructuringModelBinder : IModelBinder
	{
		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext is null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var requestBody = await bindingContext.HttpContext.ReadBodyAsStringAsync();

			var items = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestBody);

			// For the calling context, grab the Attributes of the metadata (these are the attribute of the actual controller method, basically)
			if (bindingContext
				.ModelMetadata
				.GetType()
				.GetProperty("Attributes")
				?.GetValue(bindingContext.ModelMetadata) is not ModelAttributes attributes)
			{
				throw new ArgumentException("No attributes found");
			}

			var tupleElementNamesAttribute = attributes
				.Attributes
				.OfType<TupleElementNamesAttribute>()
				.First();

			if (tupleElementNamesAttribute == null)
			{
				throw new ArgumentException("No custom tuple names specified");
			}

			// Now, construct the actual tuple from the tuple info and the raw body
			var tuple = CreateTupleFromBody(items, bindingContext.ModelType, tupleElementNamesAttribute);

			if (tuple == null)
			{
				throw new ArgumentException("Invalid arguments");
			}

			// Set the result
			bindingContext.Result = ModelBindingResult.Success(tuple);
		}

		private ITuple? CreateTupleFromBody(
			IReadOnlyDictionary<string, object> values,
			Type tupleType,
			TupleElementNamesAttribute tupleNames)
		{
			var parameters = new List<object?>();

			// Get the tuples constructor and the parameters
			var constructor = tupleType
				.GetConstructors()
				.First();
			var constructorParameters = constructor
				.GetParameters()
				.Select(x => x.ParameterType);

			foreach (var (parameterType, index) in constructorParameters.AsIndexedIterable())
			{
				// Grab the name (indexed), and try to grab it from the body. Otherwise assign it as default
				var paramTupleName = tupleNames.TransformNames[index];

				if (paramTupleName is not null && values.TryGetValue(paramTupleName, out var val))
				{
					if (val.GetType() != parameterType)
					{
						val = Convert.ChangeType(val, parameterType);
					}
					parameters.Add(val);
				}
				else
				{
					parameters.Add(DefaultFromType(parameters[index]!.GetType()));
				}
			}

			return constructor.Invoke(parameters.ToArray()) as ITuple;
		}

		private static object? DefaultFromType(Type type) => type.IsValueType ? Activator.CreateInstance(type) : default;
	}
}