using GrpcProxyGenerator.DataTypes;

namespace GrpcProxyGenerator.Service.Interface
{
	public interface IGrpcProxyEmitter
	{
		void EmitProxy(string builtProxy, ProxyMetaData metaData);
	}
}