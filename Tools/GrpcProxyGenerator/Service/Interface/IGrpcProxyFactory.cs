using System;

namespace GrpcProxyGenerator.Service.Interface
{
	public interface IGrpcProxyFactory
	{
		void GenerateProxy(Type proxyInterface);
	}
}