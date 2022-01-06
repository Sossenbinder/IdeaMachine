using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Type = System.Type;

namespace IdeaMachine.Common.Core.Extensions
{
	public static class MethodInfoExtensions
	{
		/// <summary>
		/// Fetch all the involved types of a method (Parameter AND return type). If a parameter is wrapped as Task, return
		/// it unwrapped
		/// </summary>
		/// <param name="methodInfo">MethodInfo</param>
		/// <returns>Type of params and return type</returns>
		public static IEnumerable<Type> GetInvolvedTypesUnwrapped(this MethodInfo methodInfo)
		{
			// Assemble return types and parameter types
			var involvedTypes = methodInfo.GetParameters()
				.Select(x => x.ParameterType)
				.ToList();

			involvedTypes.Add(methodInfo.ReturnType);

			// Unwrap the types now

			var cleanedTypes = new List<Type>();

			foreach (var type in involvedTypes)
			{
				// If non-generic, it's clean already
				if (!type.IsGenericType)
				{
					cleanedTypes.Add(type);
					continue;
				}

				// If it's generic, unwrap it if required.
				cleanedTypes.Add(type.CheckAndGetTaskWrappedType());
			}

			return cleanedTypes;
		}
	}
}