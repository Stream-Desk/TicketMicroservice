
using Application.Models.Mail;

namespace Application.Mail
{
    public interface IMailService
    {
        public bool SendEmail(MailData mailData);
    }
}