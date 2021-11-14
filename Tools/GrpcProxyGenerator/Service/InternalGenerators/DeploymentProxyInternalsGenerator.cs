using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GrpcProxyGenerator.DataTypes;
using GrpcProxyGenerator.Extensions;
using GrpcProxyGenerator.Helper;
using GrpcProxyGenerator.Service.InternalGenerators.Interface;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Grpc.Service.Interface;
using IdeaMachine.Common.IPC;

namespace GrpcProxyGenerator.Service.InternalGenerators
{
	internal class DeploymentProxyInternalsGenerator : IProxyInternalsGenerator
	{
		public void GenerateProxyInternals(StringBuilder stringBuilder, ProxyMetaData metaData)
		{
			GenerateConstructor(stringBuilder, metaData);
			GenerateMethods(stringBuilder, metaData);
		}

		private static void GenerateConstructor(StringBuilder stringBuilder, ProxyMetaData metaData)
		{
			stringBuilder.Tab(2).AppendLine($"public {metaData.ServiceNameShort}Proxy({typeof(IGrpcChannelProvider).GetRealFullName()} channelProvider)");
			stringBuilder.Tab(3).AppendLine(": base(channelProvider,");
			stringBuilder.Tab(4).AppendLine($"{typeof(ServiceType).GetRealFullName()}.{metaData.HostingService}Service)");
			stringBuilder.Tab(2).AppendLine("{ }");
			stringBuilder.LineBreak();
		}

		private static void GenerateMethods(StringBuilder stringBuilder, ProxyMetaData metaData)
		{
			var methods = metaData.Type.GetMethods();

			foreach (var methodInfo in methods.Where(x => x.GetCustomAttributes(typeof(OperationContractAttribute)).Any()))
			{
				AddMethod(stringBuilder, methodInfo);
			}
		}

		private static void AddMethod(StringBuilder stringBuilder, MethodInfo methodInfo)
		{
			// Needs to be cleaned, as this will always be a Task when running over grpc
			var returnType = methodInfo.ReturnType.GetRealFullName();

			var methodName = methodInfo.Name;

			var parameters = methodInfo.GetParameters();

			// Add common method visibility, return type and name
			stringBuilder.Tab(2).Append($"public {returnType} {methodName}");

			// Add parameters
			if (parameters.Any())
			{
				stringBuilder.AppendLine("(");

				for (var i = 0; i < parameters.Length; ++i)
				{
					var parameter = parameters[i];
					stringBuilder.Tab(3).Append($"{parameter.ParameterType.GetRealFullName()} {TypeNameHelper.StripGenericArtifacts(parameter.Name)}");
					stringBuilder.AppendLine(i == parameters.Length - 1 ? ")" : ",");
				}
			}
			else
			{
				stringBuilder.AppendLine("()");
			}

			// Add method forward call
			stringBuilder.Tab(3).Append($"=> {(methodInfo.ReturnType == typeof(Task) ? "Invoke" : "InvokeWithResult")}");
			stringBuilder.Append($"(service => service.{methodName}(");

			if (parameters.Any())
			{
				stringBuilder.AppendLine();
				for (var i = 0; i < parameters.Length; i++)
				{
					var parameter = parameters[i];
					stringBuilder.Tab(4).Append(TypeNameHelper.StripGenericArtifacts(parameter.Name));
					stringBuilder.AppendLine(i == parameters.Length - 1 ? "));" : ",");
				}
			}
			else
			{
				stringBuilder.AppendLine("));");
			}

			stringBuilder.AppendLine();
		}
	}
}