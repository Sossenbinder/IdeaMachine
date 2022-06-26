using System.Net;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;
using IdeaMachine.Modules.Email.Service.Interface;
using IdeaMachine.Modules.Email.Utils;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.ServiceBase;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace IdeaMachine.Modules.Email.Service
{
	public class EmailEventHandler : ServiceBaseWithoutLogger
	{
		private readonly IEmailSender _mailSender;

		private readonly IConfiguration _configuration;

		public EmailEventHandler(
			IIdeaEvents ideaEvents,
			IAccountEvents accountEvents,
			IEmailSender mailSender,
			IConfiguration configuration)
		{
			_mailSender = mailSender;
			_configuration = configuration;
			RegisterEventHandler(ideaEvents.IdeaCreated, OnIdeaCreated);
			RegisterEventHandler(accountEvents.AccountCreated, OnAccountCreated);
		}

		private async Task OnAccountCreated(AccountCreated accountCreated)
		{
			var (userName, email, verificationCode) = accountCreated;

			if (email.IsNullOrEmpty())
			{
				return;
			}

			var mail = MailFactory.CreateMail();
			mail.To.Add(new MailboxAddress($"Dear {userName}!", email));
			mail.Subject = "Thanks for signing up to IdeaMachine";

			var builder = new BodyBuilder
			{
				HtmlBody = $@"
					<h2>Your verification mail!</h2>
					<p>Dear {userName} - Thanks for signing up! Please click the link below to verify your registration.</p>
					<a href='{EnvironmentLinkGenerator.GetDomainLink(_configuration)}/VerifyEmail?userName={userName}&token={WebUtility.HtmlEncode(verificationCode)}'>Click to verify</a>
				",
			};
			mail.Body = builder.ToMessageBody();

			await _mailSender.SendMail(mail);
		}

		private async Task OnIdeaCreated(IdeaCreated ideaCreated)
		{
			var (creator, _) = ideaCreated;

			if (creator.Email.IsNullOrEmpty())
			{
				return;
			}

			var mail = MailFactory.CreateMail();
			mail.To.Add(new MailboxAddress("Dear creative!", creator.Email));
			mail.Subject = "Thank you for your idea!";
			mail.Body = new TextPart("plain")
			{
				Text = "Thanks a lot for your idea! We added it to our database and will contact you once someone is interested in picking it up"
			};

			await _mailSender.SendMail(mail);
		}
	}
}