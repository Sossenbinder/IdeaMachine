namespace IdeaMachine.Common.RuntimeSerialization.Serialize.Interface
{
	/// <summary>
	/// Marks a part of the application which will be serialized
	/// </summary>
	public interface ISectionSerializer
	{
		void SerializeSection();
	}
}