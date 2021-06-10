using System;

namespace IdeaMachine.Common.Core.Utils.IPC
{
	public abstract class ServiceResponseBase
	{
		public string? ErrorMessage { get; set; }

		public bool IsSuccess { get; set; }

		public bool IsFailure => !IsSuccess;

		protected ServiceResponseBase(bool isSuccess, string? errorMessage)
		{
			IsSuccess = isSuccess;
			ErrorMessage = errorMessage;
		}
	}

	public class ServiceResponse : ServiceResponseBase
	{
		public ServiceResponse(bool isSuccess, string? errorMessage = null)
			: base(isSuccess, errorMessage)
		{
		}

		public static ServiceResponse Success(string? errorMessage = null)
			=> new(true, errorMessage);

		public static ServiceResponse Failure(string? errorMessage = null)
			 => new(false, errorMessage);

		public static ServiceResponse<TPayload> Success<TPayload>(TPayload payload, string? errorMessage = null)
			=> new(true, payload, errorMessage);

		public static ServiceResponse<TPayload> Failure<TPayload>(TPayload payload, string? errorMessage = null)
			=> new(false, payload, errorMessage);
	}

	// Simple base class to transport the result of a backend task to the frontend and provide a way to check whether call was successful
	public class ServiceResponse<TPayload> : ServiceResponseBase
	{
		public TPayload? PayloadOrNull { get; }

		public TPayload PayloadOrFail
		{
			get
			{
				if (PayloadOrNull == null)
				{
					throw new NullReferenceException();
				}

				return PayloadOrNull;
			}
		}

		public ServiceResponse(bool isSuccess, TPayload? payload = default, string? errorMessage = null)
			: base(isSuccess, errorMessage)
		{
			PayloadOrNull = payload;
		}

		public static ServiceResponse<TPayload> Success(TPayload payload, string? errorMessage = null)
			=> new(true, payload, errorMessage);

		public static ServiceResponse<TPayload> Failure(TPayload? payload = default, string? errorMessage = null)
			=> new(false, payload, errorMessage);
	}
}