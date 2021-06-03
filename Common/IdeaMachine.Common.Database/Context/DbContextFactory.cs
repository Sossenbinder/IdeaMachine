using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IdeaMachine.Common.Database.Context
{
	public class DbContextFactory<TDbContext> : IDbContextFactory<TDbContext>
		where TDbContext : AbstractDbContext
	{
		private readonly IConfiguration _configuration;

		private readonly Func<IConfiguration, TDbContext> _contextCreator;

		public DbContextFactory(
			IConfiguration configuration,
			Func<IConfiguration, TDbContext> contextCreator)
		{
			_configuration = configuration;
			_contextCreator = contextCreator;
		}

		public TDbContext CreateDbContext() => _contextCreator(_configuration);
	}
}