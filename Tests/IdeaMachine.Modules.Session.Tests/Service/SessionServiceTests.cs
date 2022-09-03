using System;
using System.Threading.Tasks;
using Autofac;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.Account.DI;
using IdeaMachine.Modules.Session.DI;
using IdeaMachine.Modules.Session.Service.Interface;
using IdeaMachine.Tests.Core.DI;
using IdeaMachine.Tests.Core.Eventing.Extensions;
using MassTransit.Testing;
using NUnit.Framework;

namespace IdeaMachine.Modules.Session.Tests.Service
{
	[TestFixture]
	public class SessionServiceTests
	{
		private ISessionService _sessionService;

		private IAccountEvents _accountEvents;

		private BusTestHarness _harness;

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

			_accountEvents = container.Resolve<IAccountEvents>();
			_sessionService = container.Resolve<ISessionService>();
			_harness = container.Resolve<BusTestHarness>();
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

			Assert.True(_sessionService.GetSession(userId) is null);

			Assert.True(await _accountEvents.AccountSignedIn.RaiseForTest(_harness, new AccountSignedIn(account)));

			var newSession = await _sessionService.GetSession(userId);
			Assert.NotNull(newSession);
			Assert.AreEqual(account.UserId, newSession!.User.UserId);
			Assert.AreEqual(account.UserId, userId);
			Assert.AreEqual(account.Email, newSession.User.Email);
			Assert.AreEqual(account.UserName, newSession.User.UserName);
		}
	}
}