using System;
using System.Linq;
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

			return typeInterfaces.Length != 0 && typeInterfaces.Any(x => x == @interface);
		}

		public static string GetRealFullName(this Type type)
			=> type.GetRealTypeName(t => t.FullName!);

		public static string GetRealName(this Type type)
			=> type.GetRealTypeName(t => t.Name);

		public static string GetRealTypeName(this Type type, Func<Type, string> propSelector)
		{
			var toInspect = propSelector(type).Replace("+", ".");

			if (!type.IsGenericType)
			{
				return toInspect;
			}

			var sb = new StringBuilder();

			sb.Append(toInspect.Substring(0, toInspect.IndexOf('`')));
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

		public static bool IsConcrete(this Type type)
		{
			return type.IsClass && !type.IsAbstract && !type.IsInterface;
		}
	}
}