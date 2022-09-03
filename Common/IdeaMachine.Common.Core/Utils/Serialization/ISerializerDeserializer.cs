namespace IdeaMachine.Common.Core.Utils.Serialization
{
	public interface ISerializerDeserializer
	{
		string Serialize<T>(T value);

		T? Deserialize<T>(string value);
	}
}