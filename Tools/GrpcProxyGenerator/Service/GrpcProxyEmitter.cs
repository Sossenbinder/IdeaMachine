using System;
using System.IO;
using GrpcProxyGenerator.DataTypes;
using GrpcProxyGenerator.Service.Interface;

namespace GrpcProxyGenerator.Service
{
	internal class GrpcProxyEmitter : IGrpcProxyEmitter
	{
		private readonly string _solutionPath;

		public GrpcProxyEmitter(string solutionPath)
		{
			_solutionPath = solutionPath;
		}

		public void EmitProxy(string builtProxy, ProxyMetaData metaData)
		{
			var outputDir = $"{_solutionPath}\\Common\\IDeaMachine.Common.RemotingProxies\\Proxies\\";

			File.WriteAllText($"{outputDir}\\{metaData.ServiceNameShort}Proxy.cs", builtProxy);
		}
	}
}