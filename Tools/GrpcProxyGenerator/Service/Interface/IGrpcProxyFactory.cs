using System;

namespace GrpcProxyGenerator.Service.Interface
{
	internal interface IGrpcProxyFactory
	{
		void GenerateProxy(Type proxyInterface);
	}
}