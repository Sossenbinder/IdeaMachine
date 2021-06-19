using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.ModulesServiceBase.Interface;
using ProtoBuf.Meta;

namespace IdeaMachine.Service.Base.Serialization
{
	/// <summary>
	/// Dynamically registers all parameters and return types of involved
	/// IGrpcService so all of them are known to Grpc
	/// </summary>
	public class GrpcServiceTypeSerializer
	{
		private const int PreAllocatedProtoPerService = 100;

		private readonly SerializationHelper _serializationHelper;

		private readonly Dictionary<Type, int> _protoDict;

		public GrpcServiceTypeSerializer(SerializationHelper serializationHelper)
		{
			_serializationHelper = serializationHelper;

			var allGrpcServices = Assembly.GetEntryAssembly()
				.GetReferencedAssemblies()
				.Select(Assembly.Load)
				.Select(x => x.GetTypes())
				.SelectMany(x => x)
				.Where(x => x.IsInterface)
				.Where(x => x.HasInterface(typeof(IGrpcService)))
				.OrderBy(x => x.FullName)
				.ToList();

			_protoDict = new Dictionary<Type, int>();
			for (var i = 0; i < allGrpcServices!.Count; ++i)
			{
				_protoDict.Add(allGrpcServices[i], (i + 1) * PreAllocatedProtoPerService);
			}
		}

		public void SerializeGrpcServices(IEnumerable<IGrpcService> services)
		{
			// Get the types for each registered grpc service. This also includes
			// the type of the currently running service
			var grpcServicesTypes = services
				.Select(x =>
				{
					var upwardContractInterface = x
						.GetType()
						.GetFirstUpwardInterfaceChainMatchOrNull(type => type.GetCustomAttribute<ServiceContractAttribute>() is not null);
					return upwardContractInterface;
				})
				.Where(x => x is not null)
				.ToList();

			foreach (var grpcServiceType in grpcServicesTypes)
			{
				InitializeBindingsForGrpcService(grpcServiceType!);
			}
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

				// Start by registering the generic type chains
				RegisterGenericTypeChain(involvedTypes.Where(x => x.IsGenericType), grpcServiceType);

				RegisterPlainTypes(involvedTypes.Where(x => !x.IsGenericType));
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
				_serializationHelper.RegisterTypeBaseChain(GetAndIncrementIndex(grpcServiceType), genericType);
			}
		}

		private static void RegisterPlainTypes(IEnumerable<Type> plainTypes)
		{
			foreach (var type in plainTypes)
			{
				if (!type.Namespace?.StartsWith("System") ?? false)
				{
					RuntimeTypeModel.Default.Add(type);
				}
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