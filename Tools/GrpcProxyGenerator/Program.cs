using System.Linq;
using System.Reflection;
using GrpcProxyGenerator.Service;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.ModulesServiceBase.Interface;

namespace GrpcProxyGenerator
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var allGrpcServices = Assembly
				.GetEntryAssembly()
				?.GetReferencedAssemblies()
				.Select(Assembly.Load)
				.Select(x => x.GetTypes())
				.SelectMany(x => x)
				.Where(x => x.IsInterface && x.HasInterface(typeof(IGrpcService)))
				.OrderBy(x => x.FullName)
				.ToList();

			var factory = new GrpcProxyFactory(new GrpcProxyFileEmitter(args[0]), new TypeInfoProvider());

			foreach (var service in allGrpcServices!)
			{
				factory.GenerateProxy(service);
			}
		}
	}
}