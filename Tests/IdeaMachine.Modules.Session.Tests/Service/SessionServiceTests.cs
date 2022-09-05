using System;
using System.Threading.Tasks;
using Autofac;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Session.DI;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachine.Tests.Core.DI;
using MassTransit.Testing;
using NUnit.Framework;

namespace IdeaMachine.Modules.Session.Tests.Service
{
	[TestFixture]
	public class SessionServiceTests
	{
		private ISessionService _sessionService = null!;

		[SetUp]
		public async Task SetUp()
		{
			var builder = new TestContainerBuilderBuilder()
				.AddEventing()
				.Create();

			builder.RegisterModule<SessionModule>();
			builder.RegisterModule<AccountModule>();

			var container = builder.Build();
			await container.Resolve<BusTestHarness>().Start();
			_sessionService = container.Resolve<ISessionService>();
		}

		[Test]
		public async Task SessionServiceShouldProperlyAddSessionOnLogin()
		{
			var userId = Guid.NewGuid();
			var account = new Account.Abstractions.DataTypes.Account()
			{
				Email = "MyEmail",
				UserName = "TestUser",
				UserId = userId,
			};

			Assert.True(await _sessionService.GetSession(userId) is null);

			var newSession = await _sessionService.GetSession(userId);
			Assert.NotNull(newSession);
			Assert.AreEqual(account.UserId, newSession!.User.UserId);
			Assert.AreEqual(account.UserId, userId);
			Assert.AreEqual(account.Email, newSession.User.Email);
			Assert.AreEqual(account.UserName, newSession.User.UserName);
		}
	}
}