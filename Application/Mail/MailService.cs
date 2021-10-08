using System;
using System.Threading.Tasks;
using Application.Models.Mail;
using Application.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Service
{
    public class MailService : IMailService
    {
        MailSettings _mailSettings = null;

        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }

        public bool SendEmail(MailData mailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.EmailToName, _mailSettings.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = mailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
                emailClient.Authenticate(_mailSettings.EmailId, _mailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                //Log Exception Details
                return false;
            }
        }

        public Task AppointmentMail(MailData mailData)
        {
            throw new NotImplementedException();
        }
    }
}


    