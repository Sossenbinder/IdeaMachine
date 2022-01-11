using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GrpcProxySourceGenerator
{
	[Generator]
	public class GrpcProxyGenerator : ISourceGenerator
	{
		public class SyntaxReceiver : ISyntaxReceiver
		{
			public List<InterfaceDeclarationSyntax> EligibleInterfaceNodes { get; } = new List<InterfaceDeclarationSyntax>();

			public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
			{
				if (syntaxNode is InterfaceDeclarationSyntax ids && ids.AttributeLists.Any(x => x.Attributes.Any(y => y.Name.GetText().ToString() == "ServiceContract")))
				{
					EligibleInterfaceNodes.Add(ids);
				}
			}
		}

		public void Initialize(GeneratorInitializationContext context)
		{
			Debugger.Launch();
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
			{
				return;
			}

			var interfacesToGenerateFor = receiver.EligibleInterfaceNodes;

			foreach (var service in interfacesToGenerateFor)
			{
				var sb = new StringBuilder();

				var members = service.Members.OfType<MethodDeclarationSyntax>();

				var namespaceNode = ((service.Parent as NamespaceDeclarationSyntax)!).Name.ToString();
				var interfaceName = service.Identifier.Text;
				var serviceName = interfaceName.AsSpan().Slice(1).ToString();
				var className = $"{serviceName}Proxy";

				sb.Append(
					$@"
					using System.Threading.Tasks;
					using IdeaMachine.Common.Core.Utils.IPC;

					namespace IdeaMachine.Common.RemotingProxies.Proxies
					{{
						public class {className} : IdeaMachine.Common.RemotingProxies.ProxyInvocation.AbstractDeploymentProxy<{namespaceNode}.{interfaceName}>, {namespaceNode}.{interfaceName}
						{{
							public {className}(IdeaMachine.Common.Grpc.Service.Interface.IGrpcChannelProvider channelProvider)
								: base(channelProvider, IdeaMachine.Common.IPC.ServiceType.{serviceName})
							{{ }}

				");

				foreach (var call in members)
				{
					sb.Append(GenerateProxyMethod(call));
				}

				sb.Append(@"
						}
					}
				"
				);

				context.AddSource(className, SourceText.From(sb.ToString(), Encoding.UTF8));
			}
		}

		private string GenerateProxyMethod(MethodDeclarationSyntax method)
		{
			var methodIdentifier = method.Identifier.Text;

			return $@"
				public {method.ReturnType} {methodIdentifier} ({GenerateParameters(method.ParameterList)})
					=> {(method.ReturnType is GenericNameSyntax ? "InvokeWithResult" : "Invoke")}(service => service.{methodIdentifier}(
							{method.ParameterList.Parameters.Select(x => x.Identifier.Text)}
						));
			";
		}

		private static string GenerateParameters(BaseParameterListSyntax parameterList)
		{
			return string.Join(" ", parameterList
				.Parameters
				.Select(x => $"{(x.Type as SimpleNameSyntax)!.Identifier.Text} {x.Identifier.Text}"));
		}
	}
}