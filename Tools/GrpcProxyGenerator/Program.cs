using System;
using System.Linq;
using System.Reflection;
using GrpcProxyGenerator.Extensions;
using GrpcProxyGenerator.Service;
using GrpcProxyGenerator.Service.Interface;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.ModulesServiceBase.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcProxyGenerator
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			//setup our DI
			var serviceProvider = new ServiceCollection()
				.AddSingleton<IGrpcProxyEmitter, GrpcProxyEmitter>()
				.AddSingleton<IGrpcProxyFactory, GrpcProxyFactory>()
				.AddSingleton<ITypeInfoProvider, TypeInfoProvider>()
				.AddSingleton<string>(args[0])
				.BuildServiceProvider();

			var allGrpcServices = Assembly
				.GetEntryAssembly()
				.GetReferencedAssemblies()
				.Select(Assembly.Load)
				.Select(x => x.GetTypes())
				.SelectMany(x => x)
				.Where(x => x.IsInterface && x.HasInterface(typeof(IGrpcService)))
				.OrderBy(x => x.FullName)
				.ToList();

			var factoryService = serviceProvider.GetServiceOrFail<IGrpcProxyFactory>();

			foreach (var service in allGrpcServices!)
			{
				factoryService.GenerateProxy(service);
			}
		}
	}
}