using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using IdeaMachine.Common.Core.Extensions;
using IdeaMachine.Common.Core.Utils.Async;
using IdeaMachine.Modules.Email.Service.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace IdeaMachine.Modules.Email.Service
{
	public class EmailSender : IEmailSender
	{
		private readonly ILogger<EmailSender> _logger;

		private readonly IConfiguration _configuration;

		private readonly AsyncLazy<string> _credentials;

		public EmailSender(
			ILogger<EmailSender> logger,
			IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
			_credentials = new AsyncLazy<string>(() => CreateCredentials(configuration));
		}

		private async Task<string> CreateCredentials(IConfiguration configuration)
		{
			var serviceAccountInitializer = new ServiceAccountCredential
				.Initializer("")
				{
					Scopes = new List<string> { "https://www.googleapis.com/auth/gmail.send" },
					User = _configuration["GmailUserName"],
			}
				.FromPrivateKey(configuration["GmailServiceAccountKey"]);

			var credentials = new ServiceAccountCredential(serviceAccountInitializer);

			var result = await credentials.RequestAccessTokenAsync(CancellationToken.None);

			if (!result)
			{
				_logger.LogError("Failed to retrieve access token for {UserName}", "");
				throw new HttpRequestException();
			}

			return credentials.Token.AccessToken;
		}

		public async Task SendMail(MimeMessage mail)
		{
			using var smtpClient = new SmtpClient();

			try
			{
				await smtpClient.ConnectAsync("smtp.gmail.com", 587);

				await smtpClient.AuthenticateAsync(new SaslMechanismPlain(Encoding.UTF8, _configuration["GmailUserName"], _configuration["GmailPassword"]));
				var oauth2 = new SaslMechanismOAuth2(_configuration["GmailUserName"], await _credentials);
				await smtpClient.AuthenticateAsync(oauth2);
				await smtpClient.SendAsync(mail);
				await smtpClient.DisconnectAsync(true);
			}
			catch (SmtpProtocolException exc)
			{
				_logger.LogException(exc, "Failed to connect to the gmail service");
				throw;
			}
		}
	}
}