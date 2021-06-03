using System.Threading.Tasks;
using IdeaMachine.Modules.Email.Service.Interface;
using IdeaMachine.Modules.Email.Utils;
using IdeaMachine.Modules.Idea.DataTypes.Events;
using IdeaMachine.Modules.Idea.Events.Interface;
using IdeaMachine.ModulesServiceBase;
using MimeKit;

namespace IdeaMachine.Modules.Email.Service
{
	public class IdeaEventHandler : ServiceBaseWithoutLogger
	{
		private readonly IEmailSender _mailSender;

		public IdeaEventHandler(
			IIdeaEvents ideaEvents,
			IEmailSender mailSender)
		{
			_mailSender = mailSender;
			RegisterEventHandler(ideaEvents.IdeaCreated, OnIdeaCreated);
		}

		private async Task OnIdeaCreated(IdeaCreated ideaCreated)
		{
			var mail = MailFactory.CreateMail();
			mail.To.Add(new MailboxAddress("Dear creative!", ideaCreated.Idea.CreatorMail));
			mail.Subject = "Thank you for your idea!";
			mail.Body = new TextPart("plain")
			{
				Text = "Thanks a lot for your idea! We added it to our database and will contact you once someone is interested in picking it up"
			};

			await _mailSender.SendMail(mail);
		}
	}
}