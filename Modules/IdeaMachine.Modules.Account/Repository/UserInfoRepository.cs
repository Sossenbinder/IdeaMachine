using System;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Database.Context;
using IdeaMachine.Common.Database.Repository;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Entity;
using IdeaMachine.Modules.Account.Repository.Context;
using IdeaMachine.Modules.Account.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Modules.Account.Repository
{
	public class UserInfoRepository : AbstractRepository<AccountContext>, IUserInfoRepository
	{
		public UserInfoRepository(DbContextFactory<AccountContext> dbContextFactory)
			: base(dbContextFactory)
		{
		}

		public async Task<string?> GetProfilePictureUrl(Guid userId)
		{
			await using var ctx = CreateContext();

			return await ctx.UserInfo.Where(x => x.UserId == userId)
				.Select(x => x.ProfilePictureUrl)
				.FirstOrDefaultAsync();
		}

		private static async Task<UserInfoEntity> GetOrCreateUser(AccountContext ctx, Guid userId)
		{
			var user = await ctx.UserInfo.FirstOrDefaultAsync(x => x.UserId == userId);

			if (user is not null)
			{
				return user;
			}

			var userEntity = new UserInfoEntity()
			{
				UserId = userId
			};

			ctx.UserInfo.Add(userEntity);

			return userEntity;
		}

		public async Task AddProfilePictureUrlToUser(Guid userId, string pictureUrl)
		{
			await using var ctx = CreateContext();
			var user = await GetOrCreateUser(ctx, userId);

			user.ProfilePictureUrl = pictureUrl;

			await ctx.SaveChangesAsync();
		}
	}
}