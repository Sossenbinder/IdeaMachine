namespace IdeaMachine.Common.DI.DataTypes
{
	public class DataContainer<T>
	{
		public T Data { get; set; } = default!;

		public static DataContainer<T> Create(T data) => new() { Data = data };
	}
}