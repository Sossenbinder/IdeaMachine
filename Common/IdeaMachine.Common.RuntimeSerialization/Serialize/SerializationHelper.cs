﻿using System;
using ProtoBuf.Meta;

namespace IdeaMachine.Common.RuntimeSerialization
{
	public class SerializationHelper
	{
		/// <summary>
		/// Registers a type with its base chain. The individual types should already be
		/// annotated, this method will just take over chaining them together
		/// </summary>
		/// <param name="protoIndex"></param>
		/// <param name="type"></param>
		public void RegisterTypeBaseChain(int protoIndex, Type type)
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

		public MetaType SerializeType<T>()
		{
			var type = typeof(T);

			var metaType = RuntimeTypeModel.Default.Add(type);

			var index = 1200;
			foreach (var prop in type.GetProperties())
			{
				metaType.Add(index, prop.Name);
				index++;
			}

			return metaType;
		}
	}
}