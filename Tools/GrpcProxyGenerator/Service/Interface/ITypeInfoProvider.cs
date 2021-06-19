using System;
using GrpcProxyGenerator.DataTypes;

namespace GrpcProxyGenerator.Service.Interface
{
	internal interface ITypeInfoProvider
	{
		ProxyMetaData GetMetaData(Type proxyInterface);
	}
}