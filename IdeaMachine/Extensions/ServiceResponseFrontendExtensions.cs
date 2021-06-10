using System.Diagnostics.CodeAnalysis;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.Web.DataTypes.Responses;

namespace IdeaMachine.Extensions
{
	public static class ServiceResponseFrontendExtensions
	{
		public static JsonResponse ToJsonResponse([NotNull] this ServiceResponse serviceResponse)
		{
			return serviceResponse.IsSuccess ? JsonResponse.Success(serviceResponse) : JsonResponse.Error();
		}

		public static JsonDataResponse<TPayload> ToJsonResponse<TPayload>([NotNull] this ServiceResponse<TPayload> serviceResponse)
		{
			return serviceResponse.IsSuccess ? JsonResponse.Success(serviceResponse.PayloadOrNull, serviceResponse.IsSuccess) : JsonResponse.Error(serviceResponse.PayloadOrNull);
		}
	}
}