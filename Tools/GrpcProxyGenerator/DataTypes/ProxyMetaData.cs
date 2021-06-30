using System;

namespace GrpcProxyGenerator.DataTypes
{
	public class ProxyMetaData
	{
		public Type Type { get; set; }

		public string InterfaceNameShort { get; set; }

		public string ServiceNameShort { get; set; }

		public string HostingService { get; set; }
	}
}