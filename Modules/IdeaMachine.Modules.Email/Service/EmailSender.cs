using System.Net;
using System.Threading.Tasks;
using IdeaMachine.Modules.Email.Service.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace IdeaMachine.Modules.Email.Service
{
	public class EmailSender : IEmailSender
	{
		private readonly ICredentials _credentials;

		public EmailSender(IConfiguration configuration)
		{
			_credentials = new NetworkCredential(configuration["MailJet_UserName"], configuration["MailJet_Password"]);
		}

		public async Task SendMail(MimeMessage mail)
		{
			using var smtpClient = new SmtpClient();
			await smtpClient.ConnectAsync("in-v3.mailjet.com", 465, true);
			await smtpClient.AuthenticateAsync(_credentials);
			await smtpClient.SendAsync(mail);
			await smtpClient.DisconnectAsync(true);
		}
	}
}