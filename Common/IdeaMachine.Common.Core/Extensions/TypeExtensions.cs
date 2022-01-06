using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Extensions
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Accepts a type and returns it stripped from a Task<> wrap if available
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Type CheckAndGetTaskWrappedType(this Type type)
			=> type.GetGenericTypeDefinition() == typeof(Task<>) ? type.GetGenericArguments()[0] : type;

		public static bool HasInterface(this Type type, Type @interface)
		{
			var typeInterfaces = type.GetInterfaces();

			return typeInterfaces.Any(x => x == @interface);
		}

		public static string GetRealFullName(this Type type)
			=> type.GetRealTypeName(t => t.FullName!);

		public static string GetRealName(this Type type)
			=> type.GetRealTypeName(t => t.Name);

		/// <summary>
		/// Sanitizes a type name and removes artifacts like + or the generic `
		/// </summary>
		/// <param name="type">Type to sanitize</param>
		/// <param name="propSelector">Selector of the name parameter (Might be .Name or .FullName for example)</param>
		/// <returns>Sanitized type name</returns>
		public static string GetRealTypeName(this Type type, Func<Type, string> propSelector)
		{
			var toInspect = propSelector(type).Replace("+", ".");

			if (!type.IsGenericType)
			{
				return toInspect;
			}

			var sb = new StringBuilder();

			sb.Append(toInspect[..toInspect.IndexOf('`')]);
			sb.Append('<');

			var appendComma = false;
			foreach (var argType in type.GetGenericArguments())
			{
				if (appendComma)
				{
					sb.Append(',');
				}
				sb.Append(GetRealTypeName(argType, propSelector));
				appendComma = true;
			}
			sb.Append('>');

			return sb.ToString();
		}

		/// <summary>
		/// Walks a types inheritance chain upwards, checking the matchFunc for every item and returning the first hit.
		/// </summary>
		public static Type? GetFirstUpwardInterfaceChainMatchOrNull(this Type sourceType, Func<Type, bool> matchFunc)
		{
			if (matchFunc(sourceType))
			{
				return sourceType;
			}

			return sourceType
				.GetInterfaces()
				.Select(@interface => @interface.GetFirstUpwardInterfaceChainMatchOrNull(matchFunc))
				.FirstOrDefault(result => result is not null);
		}

		/// <summary>
		/// Checks if a type is concrete, as in, no abstract or interface type
		/// </summary>
		public static bool IsConcrete(this Type type)
		{
			return type.IsClass && !type.IsAbstract && !type.IsInterface;
		}

		// Checks if a type is native, as in, part of the System.* namespace
		public static bool IsNativeType(this Type type) => type.Namespace?.StartsWith("System", StringComparison.InvariantCultureIgnoreCase) ?? false;

		/// <summary>
		/// Retrieves all properties, including recursive nested properties based on a filter
		/// </summary>
		/// <param name="type"></param>
		/// <param name="filterPredicate">Potential predicate to check </param>
		/// <param name="flags">Binding flags</param>
		/// <returns></returns>
		public static List<Type> GetNestedProperties(this Type type, Func<Type, bool>? filterPredicate = null,
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
		{
			var filter = filterPredicate ?? IsConcrete;

			var result = new List<Type>();
			foreach (var propType in type.GetProperties(flags).Select(x => x.PropertyType))
			{
				if (!filter(propType))
				{
					result.Add(propType);
				}

				result.AddRange(GetNestedPropertiesInternal(propType, filter, flags));
			}

			return result;
		}

		private static IEnumerable<Type> GetNestedPropertiesInternal(this Type type, Func<Type, bool> filterPredicate, BindingFlags flags)
		{
			if (filterPredicate(type))
			{
				return new List<Type>();
			}

			return type
				.GetProperties(flags)
				.SelectMany(x => x.PropertyType.GetNestedPropertiesInternal(filterPredicate, flags))
				.ToList();
		}
	}
}