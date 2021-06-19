using GrpcProxyGenerator.DataTypes;

namespace GrpcProxyGenerator.Service.Interface
{
	internal interface IGrpcProxyEmitter
	{
		void EmitProxy(string builtProxy, ProxyMetaData metaData);
	}
}