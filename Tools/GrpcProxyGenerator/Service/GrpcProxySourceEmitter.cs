using GrpcProxyGenerator.DataTypes;
using GrpcProxyGenerator.Service.Interface;
using Microsoft.CodeAnalysis;

namespace GrpcProxyGenerator.Service
{
	public class GrpcProxySourceEmitter : IGrpcProxyEmitter
	{
		private readonly GeneratorExecutionContext _context;

		public GrpcProxySourceEmitter(GeneratorExecutionContext context) => _context = context;

		public void EmitProxy(string builtProxy, ProxyMetaData metaData)
		{
			_context.AddSource(metaData.ServiceNameShort, builtProxy);
		}
	}
}