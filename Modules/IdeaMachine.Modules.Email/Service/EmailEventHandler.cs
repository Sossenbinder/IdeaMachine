using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Modules.Account.DataTypes.Events;
using IdeaMachine.Modules.Account.Events.Interface;
using IdeaMachine.Modules.Email.Service.Interface;
using IdeaMachine.Modules.Email.Utils;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.ModulesServiceBase;
using MimeKit;

namespace IdeaMachine.Modules.Email.Service
{
	public class EmailEventHandler : ServiceBaseWithoutLogger
	{
		private readonly IEmailSender _mailSender;

		public EmailEventHandler(
			IIdeaEvents ideaEvents,
			IAccountEvents accountEvents,
			IEmailSender mailSender)
		{
			_mailSender = mailSender;
			RegisterEventHandler(ideaEvents.IdeaCreated, OnIdeaCreated);
			RegisterEventHandler(accountEvents.AccountCreated, OnAccountCreated);
		}

		private Task OnAccountCreated(AccountCreated obj)
		{
			throw new System.NotImplementedException();
		}

		private async Task OnIdeaCreated(IdeaCreated ideaCreated)
		{
			if (ideaCreated.Creator.Email.IsNullOrEmpty())
			{
				return;
			}

			var mail = MailFactory.CreateMail();
			mail.To.Add(new MailboxAddress("Dear creative!", ideaCreated.Creator.Email));
			mail.Subject = "Thank you for your idea!";
			mail.Body = new TextPart("plain")
			{
				Text = "Thanks a lot for your idea! We added it to our database and will contact you once someone is interested in picking it up"
			};

			await _mailSender.SendMail(mail);
		}
	}
}