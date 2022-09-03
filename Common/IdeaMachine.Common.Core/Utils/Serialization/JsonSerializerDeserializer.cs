using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdeaMachine.Common.Core.Utils.Serialization
{
	public class JsonSerializerDeserializer : ISerializerDeserializer
	{
		private readonly JsonSerializerOptions _options;

		public JsonSerializerDeserializer(params JsonConverter[] converters)
		{
			_options = new JsonSerializerOptions();
			foreach (var converter in converters)
			{
				_options.Converters.Add(converter);
			}
		}

		public string Serialize<T>(T value)
		{
			return JsonSerializer.Serialize(value, _options);
		}

		public T? Deserialize<T>(string value)
		{
			return JsonSerializer.Deserialize<T>(value, _options);
		}
	}
}