using System;

namespace IdeaMachine.ModulesServiceBase.Attributes
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class GrpcServiceIdentifierAttribute : Attribute
	{
		public int Identifier { get; }

		public GrpcServiceIdentifierAttribute(int identifier) => Identifier = identifier;
	}
}