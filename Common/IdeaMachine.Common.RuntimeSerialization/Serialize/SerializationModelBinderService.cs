using System.Collections.Generic;
using System.Diagnostics;
using IdeaMachine.Common.RuntimeSerialization.Serialize.Interface;
using Microsoft.Extensions.Logging;
using ProtoBuf.Meta;

namespace IdeaMachine.Common.RuntimeSerialization.Serialize
{
	/// <summary>
	/// Auto-binds all return values & params used by grpc service
	/// </summary>
	public class SerializationModelBinderService
	{
		private readonly ILogger _logger;

		private readonly IEnumerable<ISectionSerializer> _sectionSerializers;

		public SerializationModelBinderService(
			ILogger<SerializationModelBinderService> logger,
			IEnumerable<ISectionSerializer> sectionSerializers)
		{
			_logger = logger;
			_sectionSerializers = sectionSerializers;
		}

		public void InitializeProtoSerializer()
		{
			var serializationStopwatch = Stopwatch.StartNew();

			_logger.LogInformation("Starting to initialize proto serialization model");

			foreach (var serializer in _sectionSerializers)
			{
				serializer.SerializeSection();
			}

			serializationStopwatch.Stop();
			_logger.LogInformation("Initialization of serialization model service took {elapsed}", serializationStopwatch.Elapsed);

			RuntimeTypeModel.Default.AutoAddMissingTypes = true;
			RuntimeTypeModel.Default.AutoCompile = true;
		}
	}
}