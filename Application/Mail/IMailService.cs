using System.Threading.Tasks;
using Application.Models.Mail;

namespace Application.Service
{
    public interface IMailService
    {
        public bool SendEmail(MailData mailData);
       
    }
}