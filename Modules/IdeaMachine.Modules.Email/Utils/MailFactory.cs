using MimeKit;

namespace IdeaMachine.Modules.Email.Utils
{
	public static class MailFactory
	{
		/// <summary>
		/// Creates a MimeMessage with the Ideamachine defaults filled out
		/// </summary>
		/// <returns></returns>
		public static MimeMessage CreateMail()
		{
			var mailMessage = new MimeMessage();
			mailMessage.From.Add(new MailboxAddress("Ideamachine", "ideamachinenews@gmail.com"));

			return mailMessage;
		}
	}
}