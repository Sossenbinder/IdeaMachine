using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Modules.Session.Abstractions.DataTypes;
using IdeaMachine.ModulesServiceBase.Interface;
using Microsoft.Extensions.Logging;
using ProtoBuf.Meta;

namespace IdeaMachine.Common.RuntimeSerialization
{
	/// <summary>
	/// Auto-binds all return values & params used by grpc service
	/// </summary>
	public class SerializationModelBinderService
	{
		private readonly ILogger _logger;

		private readonly IEnumerable<IGrpcService> _services;

		private readonly GrpcServiceTypeSerializer _grpcServiceTypeSerializer;

		public SerializationModelBinderService(
			ILogger<SerializationModelBinderService> logger,
			IEnumerable<IGrpcService> services,
			GrpcServiceTypeSerializer grpcServiceTypeSerializer)
		{
			_logger = logger;
			_services = services;
			_grpcServiceTypeSerializer = grpcServiceTypeSerializer;

			Start();
		}

		public void Start()
		{
			var stopWatch = Stopwatch.StartNew();

			_grpcServiceTypeSerializer.SerializeGrpcServices(_services);

			BindUserSessionContainers();

			stopWatch.Stop();
			_logger.LogInformation($"Initialization of serialization model service took {stopWatch.Elapsed}");

			RuntimeTypeModel.Default.AutoAddMissingTypes = true;
			RuntimeTypeModel.Default.AutoCompile = true;
		}

		private static void BindUserSessionContainers()
		{
			var containerType = typeof(UserSessionContainer);
			var index = 700;

			var containerImplementers = Assembly
				.GetEntryAssembly()
				.GetReferencedAssemblies()
				.Select(Assembly.Load)
				.SelectMany(x => x.GetTypes())
				.Where(x => x.BaseType == containerType)
				.OrderBy(type => type.Name)
				.ToList();

			if (containerImplementers.IsNullOrEmpty())
			{
				return;
			}

			var baseMetaData = RuntimeTypeModel.Default.Add(containerType);

			foreach (var implementer in containerImplementers!)
			{
				baseMetaData.AddSubType(index, implementer);
				index++;
			}
		}
	}
}