using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.RuntimeSerialization.Extensions;
using IdeaMachine.Common.RuntimeSerialization.Serialize.Interface;
using IdeaMachine.ModulesServiceBase.Attributes;
using IdeaMachine.ModulesServiceBase.Interface;
using Microsoft.Extensions.Logging;
using ProtoBuf.Meta;

namespace IdeaMachine.Common.RuntimeSerialization.Serialize
{
	/// <summary>
	/// Dynamically registers all parameters and return types of involved
	/// IGrpcService so all of them are known to Grpc
	/// </summary>
	public class GrpcServiceTypeSerializer : ISectionSerializer
	{
		private readonly ILogger _logger;

		private readonly IEnumerable<IGrpcService> _services;

		private const int PreAllocatedProtoPerService = 100;

		private Dictionary<Type, int> _protoDict = null!;

		public GrpcServiceTypeSerializer(
			ILogger<GrpcServiceTypeSerializer> logger,
			IEnumerable<IGrpcService> services)
		{
			_logger = logger;
			_services = services;
		}

		public void SerializeSection()
		{
			// Get the types for each registered grpc service. This also includes
			// the type of the currently running service
			List<Type> grpcServicesTypes = _services
				.Select(x => x.GetType().GetFirstUpwardInterfaceChainMatchOrNull(type => type.GetCustomAttribute<ServiceContractAttribute>() is not null))
				.Where(x => x is not null)
				.ToList()!;

			PrepareProtoReservations(grpcServicesTypes);

			foreach (var grpcServiceType in grpcServicesTypes)
			{
				InitializeBindingsForGrpcService(grpcServiceType!);
			}
		}

		private void PrepareProtoReservations(IEnumerable<Type> grpcServices)
		{
			_protoDict = grpcServices
				.ToDictionary(
					x => x, x =>
					{
						var identifierAttribute = x.GetCustomAttribute<GrpcServiceIdentifierAttribute>();

						if (identifierAttribute is not null)
						{
							return identifierAttribute.Identifier * PreAllocatedProtoPerService;
						}

						var exception = new ArgumentException($"Service of type {x.FullName} does not have a {nameof(GrpcServiceIdentifierAttribute)} attached to it");
						_logger.LogException(exception);
						throw exception;
					});
		}

		private void InitializeBindingsForGrpcService(Type grpcServiceType)
		{
			var operationContractMethods = grpcServiceType
				.GetMethods()
				.Where(method => method.GetCustomAttribute<OperationContractAttribute>() is not null);

			// Get all the grpc methods -> All of these are exposed because we are
			// working on the interface anyways
			foreach (var method in operationContractMethods)
			{
				var involvedTypes = method
					.GetInvolvedTypesUnwrapped()
					.ToList();

				var genericTypes = involvedTypes.Where(x => x.IsGenericType).ToList();
				// Start by registering the generic type chains
				RegisterGenericTypeChain(genericTypes, grpcServiceType);
				involvedTypes.RemoveRange(genericTypes);

				// Now remove the types which are plain types, but are part of System.*
				involvedTypes.RemoveWhere(x => x.IsNativeType());

				RegisterPlainTypes(involvedTypes);

				// The types themselves are registered now -
				var externalTypes = involvedTypes.SelectMany(x => x.GetNestedProperties(y => y.IsNativeType() || y.Namespace!.StartsWith("IdeaMachine", StringComparison.InvariantCultureIgnoreCase))).ToList();
				RegisterPlainTypes(externalTypes);
			}
		}

		/// <summary>
		/// Register all generic types on a service method declaration. E.g.
		/// chains <see cref="ServiceResponse"/> to the respective base chain
		/// </summary>
		private void RegisterGenericTypeChain(IEnumerable<Type> genericTypes, Type grpcServiceType)
		{
			foreach (var genericType in genericTypes)
			{
				genericType.RegisterTypeBaseChain(GetAndIncrementIndex(grpcServiceType));
			}
		}

		private static void RegisterPlainTypes(IEnumerable<Type> plainTypes)
		{
			foreach (var type in plainTypes)
			{
				RuntimeTypeModel.Default.Add(type);
			}
		}

		private int GetAndIncrementIndex(Type type)
		{
			var index = _protoDict[type];

			_protoDict[type] = index + 1;

			return index;
		}
	}
}