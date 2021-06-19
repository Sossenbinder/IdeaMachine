using System.Text;
using GrpcProxyGenerator.DataTypes;

namespace GrpcProxyGenerator.Service.InternalGenerators.Interface
{
	internal interface IProxyInternalsGenerator
	{
		void GenerateProxyInternals(StringBuilder stringBuilder, ProxyMetaData metaData);
	}
}