using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Common.Database.Extensions
{
	public static class DbContextExtensions
	{
		public static async Task<bool> SaveChangesAsyncWithResult(this DbContext dbContext, CancellationToken cancellationToken = default)
		{
			return await dbContext.SaveChangesAsync(cancellationToken) != 0;
		}
	}
}