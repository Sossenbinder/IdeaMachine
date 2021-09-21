using System;
using System.Collections.Generic;
using System.Linq;
using IdeaMachine.Common.Core.Utils.IPC;
using IdeaMachine.Common.Web.DataTypes.Responses;

namespace IdeaMachine.Extensions
{
	public static class ServiceResponseFrontendExtensions
	{
		public static JsonResponse ToJsonResponse(this ServiceResponse serviceResponse)
		{
			return serviceResponse.IsSuccess 
				? JsonResponse.Success(serviceResponse) 
				: JsonResponse.Error();
		}

		public static JsonDataResponse<TPayload> ToJsonDataResponse<TPayload>(this ServiceResponse<TPayload> serviceResponse)
		{
			return serviceResponse.IsSuccess 
				? JsonResponse.Success(serviceResponse.PayloadOrFail, serviceResponse.IsSuccess) 
				: JsonResponse.Error(serviceResponse.PayloadOrNull);
		}


		public static JsonDataResponse<TNewModel> ToJsonDataResponse<TPayload, TNewModel>(this ServiceResponse<TPayload> serviceResponse, Func<TPayload, TNewModel> transformer)
		{
			return serviceResponse.IsSuccess
				? JsonResponse.Success(transformer(serviceResponse.PayloadOrFail), serviceResponse.IsSuccess)
				: JsonResponse.Error(serviceResponse.PayloadOrNull is null ? default : transformer(serviceResponse.PayloadOrFail));
		}

		public static JsonDataResponse<List<TNewModel>> ToJsonDataResponse<TPayload, TNewModel>(this ServiceResponse<List<TPayload>> serviceResponse, Func<TPayload, TNewModel> transformer)
		{
			return serviceResponse.IsSuccess
				? JsonResponse.Success(serviceResponse.PayloadOrFail.Select(transformer).ToList(), serviceResponse.IsSuccess)
				: JsonResponse.Error(serviceResponse.PayloadOrNull is null ? default : serviceResponse.PayloadOrFail.Select(transformer).ToList());
		}
	}
}