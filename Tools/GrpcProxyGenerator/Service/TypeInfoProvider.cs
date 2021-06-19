using System;
using GrpcProxyGenerator.DataTypes;
using GrpcProxyGenerator.Service.Interface;

namespace GrpcProxyGenerator.Service
{
	internal class TypeInfoProvider : ITypeInfoProvider
	{
		public ProxyMetaData GetMetaData(Type proxyInterface)
		{
			var metaData = new ProxyMetaData
			{
				Type = proxyInterface,
				InterfaceNameShort = proxyInterface.Name
			};

			metaData.ServiceNameShort = metaData.InterfaceNameShort[1..];

			var assemblyName = metaData.Type.Assembly.GetName().Name!;
			metaData.HostingService = assemblyName[(assemblyName.LastIndexOf('.') + 1)..];

			return metaData;
		}
	}
}