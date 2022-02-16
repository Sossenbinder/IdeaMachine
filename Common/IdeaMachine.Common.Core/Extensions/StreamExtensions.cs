using System.IO;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Streams;

namespace IdeaMachine.Common.Core.Extensions
{
	public static class StreamExtensions
	{
		public static async Task<string> ReadAsStringAsync(this Stream stream)
		{
			using var streamReader = new StreamReader(stream);

			return await streamReader.ReadToEndAsync();
		}
	}
}