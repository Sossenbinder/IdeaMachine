namespace IdeaMachine.Common.Web.DataTypes
{
	public record JsonResponsePayload<T>(bool Success, T? Data);

	public record JsonResponsePayload(bool Success, object? Data) : JsonResponsePayload<object>(Success, Data);
}