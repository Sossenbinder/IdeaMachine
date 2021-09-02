using System;
using System.IO;
using GrpcProxyGenerator.DataTypes;
using GrpcProxyGenerator.Service.Interface;

namespace GrpcProxyGenerator.Service
{
	public class GrpcProxyFileEmitter : IGrpcProxyEmitter
	{
		private readonly string _solutionPath;

		public GrpcProxyFileEmitter(string solutionPath)
		{
			_solutionPath = solutionPath;
		}

		public void EmitProxy(string builtProxy, ProxyMetaData metaData)
		{
			var outputDir = $"{_solutionPath}\\Common\\IdeaMachine.Common.RemotingProxies\\Proxies\\";

			File.WriteAllText($"{outputDir}\\{metaData.ServiceNameShort}Proxy.cs", builtProxy);
		}
	}
}