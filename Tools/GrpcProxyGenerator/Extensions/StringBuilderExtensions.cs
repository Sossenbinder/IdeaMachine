using System.Text;

namespace GrpcProxyGenerator.Extensions
{
	public static class StringBuilderExtensions
	{
		public static StringBuilder Tab(this StringBuilder stringBuilder) => stringBuilder.Append("\t");

		public static StringBuilder Tab(this StringBuilder stringBuilder, int amount)
		{
			for (; amount > 0; amount--)
			{
				stringBuilder.Tab();
			}

			return stringBuilder;
		}

		public static StringBuilder DoubleTab(this StringBuilder stringBuilder) => stringBuilder.Tab().Tab();

		public static StringBuilder TripleTab(this StringBuilder stringBuilder) => stringBuilder.Tab().Tab().Tab();

		public static StringBuilder LineBreak(this StringBuilder stringBuilder) => stringBuilder.AppendLine();
	}
}