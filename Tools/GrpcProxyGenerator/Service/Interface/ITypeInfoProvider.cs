using System;
using GrpcProxyGenerator.DataTypes;

namespace GrpcProxyGenerator.Service.Interface
{
	public interface ITypeInfoProvider
	{
		ProxyMetaData GetMetaData(Type proxyInterface);
	}
}