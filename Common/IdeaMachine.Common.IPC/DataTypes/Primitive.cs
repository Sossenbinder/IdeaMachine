using System;
using ProtoBuf;

namespace IdeaMachine.Common.IPC.DataTypes
{
	[ProtoContract]
	public class Primitive<T>
		where T : struct
	{
		[ProtoMember(1)]
		public T Value { get; set; }

		public static implicit operator Primitive<T>(T val) => new() { Value = val };

		public static implicit operator T(Primitive<T> primitive) => primitive.Value;

		public override string ToString() => Value.ToString() ?? throw new InvalidOperationException("No Value set");
	}
}