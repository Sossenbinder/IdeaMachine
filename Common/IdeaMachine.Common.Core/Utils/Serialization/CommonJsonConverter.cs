using System;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IdeaMachine.Common.Core.Utils.Serialization
{
	public class CommonJsonConverter<TAbstraction, TImplementation> : System.Text.Json.Serialization.JsonConverter<TAbstraction>
		where TImplementation : TAbstraction
	{
		private readonly Type _abstractionType;

		public CommonJsonConverter()
		{
			_abstractionType = typeof(TAbstraction);
		}

		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert == _abstractionType;
		}

		public override TAbstraction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return JsonSerializer.Deserialize<TImplementation>(ref reader, options);
		}

		public override void Write(Utf8JsonWriter writer, TAbstraction value, JsonSerializerOptions? options)
		{
			JsonSerializer.Serialize(writer, (TImplementation)value!, options);
		}
	}
}