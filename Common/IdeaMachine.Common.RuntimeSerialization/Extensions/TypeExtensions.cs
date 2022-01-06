using System;
using ProtoBuf.Meta;

namespace IdeaMachine.Common.RuntimeSerialization.Extensions
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Registers a type with its base chain. The individual types should already be
		/// annotated, this method will just take over chaining them together
		/// </summary>
		/// <param name="protoIndex"></param>
		/// <param name="type"></param>
		public static void RegisterTypeBaseChain(this Type type, int protoIndex)
		{
			while (true)
			{
				var baseType = type.BaseType;

				if (baseType == null || baseType == typeof(object))
				{
					return;
				}

				var baseMetaData = RuntimeTypeModel.Default.Add(baseType);

				baseMetaData.AddSubType(protoIndex, type);
				protoIndex++;

				type = baseType;
			}
		}
	}
}