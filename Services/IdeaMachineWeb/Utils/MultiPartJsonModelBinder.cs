using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdeaMachineWeb.Utils
{
	public class MultiPartJsonModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext is null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var valueProviderResult = bindingContext.ValueProvider.GetValue("requestData");
			if (valueProviderResult == ValueProviderResult.None)
			{
				return Task.CompletedTask;
			}

			bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

			// Attempt to convert the input value
			var valueAsString = valueProviderResult.FirstValue;

			if (valueAsString is null)
			{
				return Task.CompletedTask;
			}

			var result = Newtonsoft.Json.JsonConvert.DeserializeObject(valueAsString, bindingContext.ModelType);
			if (result is null)
			{
				return Task.CompletedTask;
			}

			bindingContext.Result = ModelBindingResult.Success(result);
			return Task.CompletedTask;
		}
	}
}