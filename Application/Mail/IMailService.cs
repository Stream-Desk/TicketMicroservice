
using Application.Models;


namespace Application.Services
{
    public interface IMailService
    {
        bool SendEmail(MailData mailData);

    }
}