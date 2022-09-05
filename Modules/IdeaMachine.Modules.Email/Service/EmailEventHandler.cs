using System.Threading.Tasks;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Modules.Email.Service.Interface;
using IdeaMachine.Modules.Email.Utils;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.Modules.ServiceBase;
using MimeKit;

namespace IdeaMachine.Modules.Email.Service
{
	public class EmailEventHandler : ServiceBaseWithoutLogger
	{
		private readonly IEmailSender _mailSender;

		public EmailEventHandler(
			IIdeaEvents ideaEvents,
			IEmailSender mailSender)
		{
			_mailSender = mailSender;
			RegisterEventHandler(ideaEvents.IdeaCreated, OnIdeaCreated);
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