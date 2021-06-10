using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdeaMachine.Common.Web.DataTypes.Responses
{
	public class JsonResponse : JsonResult
	{
		private readonly HttpStatusCode _statusCode;

		protected JsonResponse(object? data, bool success, HttpStatusCode statusCode)
			: base(new
			{
				success,
				data
			})
		{
			_statusCode = statusCode;
		}

		public static JsonResponse Success(object? data = null, bool internalSuccess = true)
		{
			return new(data, internalSuccess, HttpStatusCode.OK);
		}

		public static JsonResponse Error(object? data = null)
		{
			return new(data, false, HttpStatusCode.OK);
		}

		public static JsonDataResponse<TPayload> Success<TPayload>(TPayload? data = default, bool internalSuccess = true)
		{
			return new(data, internalSuccess, HttpStatusCode.OK);
		}

		public static JsonDataResponse<TPayload> Error<TPayload>(TPayload? data = default)
		{
			return new(data, false, HttpStatusCode.OK);
		}

		public override Task ExecuteResultAsync([NotNull] ActionContext context)
		{
			context.HttpContext.Response.StatusCode = (int)_statusCode;
			return base.ExecuteResultAsync(context);
		}
	}

	public class JsonDataResponse<T> : JsonResponse
	{
		public JsonDataResponse(T? data, bool success, HttpStatusCode statusCode)
			: base(data, success, statusCode)
		{
		}

		public static JsonDataResponse<T> Success(T? data = default, bool internalSuccess = true)
		{
			return new(data, internalSuccess, HttpStatusCode.OK);
		}

		public static JsonDataResponse<T> Error(T? data = default)
		{
			return new(data, false, HttpStatusCode.OK);
		}
	}
}