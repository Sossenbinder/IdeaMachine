using System;
using ProtoBuf;

namespace IdeaMachine.Common.Core.Utils.IPC
{
	[ProtoContract]
	[ProtoInclude(80, typeof(ServiceResponse))]
	public abstract class ServiceResponseBase
	{
		[ProtoMember(1)]
		public string? ErrorMessage { get; set; }

		[ProtoMember(2)]
		public bool IsSuccess { get; set; }

		public bool IsFailure => !IsSuccess;

		// Implicitly used by protobuf-net, so the lib can use this instead of having to fallback onto the non-parameterless constructor
		protected ServiceResponseBase()
		{
		}

		protected ServiceResponseBase(bool isSuccess, string? errorMessage)
		{
			IsSuccess = isSuccess;
			ErrorMessage = errorMessage;
		}
	}

	[ProtoContract]
	public class ServiceResponse : ServiceResponseBase
	{
		// Implicitly used by protobuf-net, so the lib can use this instead of having to fallback onto the non-parameterless constructor
		// ReSharper disable once UnusedMember.Global
		protected ServiceResponse()
		{
		}

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

	[ProtoContract]
	public class ServiceResponse<TPayload> : ServiceResponseBase
	{
		[ProtoMember(3)]
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

		// Implicitly used by protobuf-net, so the lib can use this instead of having to fallback onto the non-parameterless constructor
		// ReSharper disable once UnusedMember.Local
		private ServiceResponse()
		{
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