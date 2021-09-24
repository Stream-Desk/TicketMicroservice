
using Application.Models;
using Application.Tickets;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IMailService
    {
        bool SendEmail(MailData mailData);
        

        
    }
}