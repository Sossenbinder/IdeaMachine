using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Utils.Environment;
using IdeaMachine.Common.Core.Utils.IPC;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Common.Grpc.Interceptors
{
	public class GrpcServiceResponseInterceptor : Interceptor
	{
		private readonly ILogger<GrpcServiceResponseInterceptor> _logger;

		private readonly Type ServiceResponseType = typeof(ServiceResponseBase);

		public GrpcServiceResponseInterceptor(ILogger<GrpcServiceResponseInterceptor> logger)
		{
			_logger = logger;
		}

		public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
			TRequest request, 
			ServerCallContext context,
			UnaryServerMethod<TRequest, TResponse> continuation)
		{
			try
			{
				return await continuation(request, context);
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);

				if (EnvHelper.GetDeployment() == Environments.Development)
				{
					throw;
				}

				var responseType = typeof(TResponse);
				if (responseType.BaseType != ServiceResponseType)
				{
					throw;
				}

				return (Activator.CreateInstance(responseType, true) as TResponse)!;
			}
		}
	}
}
