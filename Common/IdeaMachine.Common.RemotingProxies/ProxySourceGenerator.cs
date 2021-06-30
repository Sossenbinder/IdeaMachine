using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using GrpcProxyGenerator.Service;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.ModulesServiceBase.Interface;
using Microsoft.CodeAnalysis;

namespace IdeaMachine.Common.RemotingProxies
{
	[Generator]
	public class ProxySourceGenerator : ISourceGenerator
	{
		private List<Type> _grpcServices;

		public void Initialize(GeneratorInitializationContext context)
		{
#if DEBUG
			if (!Debugger.IsAttached)
			{
				Debugger.Launch();
			}
#endif
			_grpcServices = Assembly
				.GetEntryAssembly()
				?.GetReferencedAssemblies()
				.Select(Assembly.Load)
				.Select(x => x.GetTypes())
				.SelectMany(x => x)
				.Where(x => x.IsInterface && x.HasInterface(typeof(IGrpcService)))
				.OrderBy(x => x.FullName)
				.ToList()!;
		}

		public void Execute(GeneratorExecutionContext context)
		{
#if DEBUG
			if (!Debugger.IsAttached)
			{
				Debugger.Launch();
			}
#endif
			var factory = new GrpcProxyFactory(new GrpcProxySourceEmitter(context), new TypeInfoProvider());

			foreach (var service in _grpcServices)
			{
				factory.GenerateProxy(service);
			}
		}
	}
}