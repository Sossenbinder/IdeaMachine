using System.Threading.Tasks;
using MimeKit;

namespace IdeaMachine.Modules.Email.Service.Interface
{
	public interface IEmailSender
	{
		Task SendMail(MimeMessage mail);
	}
}