using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Mail;
using Application.Models;
using Application.Models.Appointments;
using Application.Models.Mail;
using Application.Settings;
using Domain.Appointments;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Appointments
{
    public class AppointmentService : IAppointmentService, IMailService
    {
        private readonly IAppointmentCollection _appointmentCollection;
        MailSettings _mailSettings = null;

        public AppointmentService(IAppointmentCollection appointmentCollection, IOptions<MailSettings> options)
        {
            _appointmentCollection = appointmentCollection;
            _mailSettings = options.Value;
        }
        
        public async Task<List<GetAppointmentsModel>> GetAppointments(CancellationToken cancellationToken = default)
        {
            var searchResults = await _appointmentCollection.GetAppointments(cancellationToken);
            if (searchResults == null || searchResults.Count < 1)
            {
                return new List<GetAppointmentsModel>();
            }

            var result = new List<GetAppointmentsModel>();

            foreach (var searchResult in searchResults)
            {
                var model = new GetAppointmentsModel()
                {
                    Id = searchResult.Id, 
                    Date = searchResult.Date,
                    StartTime = searchResult.StartTime,
                    EndTime = searchResult.EndTime,
                    Summary = searchResult.Summary
                };
                result.Add(model);
            }
            return result;
        }

        public async Task<GetAppointmentsModel> GetAppointmentById(string appointmentId, CancellationToken cancellationToken = default)
        {
            // String validation
            if (string.IsNullOrWhiteSpace(appointmentId))
            {
                throw new Exception("Draft not found");
            }

            var cursor = await _appointmentCollection.GetAppointmentById(appointmentId, cancellationToken);
            if (cursor == null)
            {
                return new GetAppointmentsModel();
            }

            var result = new GetAppointmentsModel()
            {
                Id = cursor.Id, 
                Date = cursor.Date,
                StartTime = cursor.StartTime,
                EndTime = cursor.EndTime,
                Summary = cursor.Summary
            };
            return result;
        }

        public async Task<GetAppointmentsModel> CreateAppointment(AddAppointmentModel model, CancellationToken cancellationtoken = default)
        {
            // Validation of the Appointment Model
            if (model == null)
            {
                throw new Exception("Appointment Details Not Found");
            }
            
            // Map the model to the domain Entity

            var appointment = new Appointment()
            {   
                Date = model.Date,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Summary = model.Summary,
                UserEmail = model.UserEmail,
                UserName = model.UserName
            };

            var cursor = await _appointmentCollection.CreateAppointment(appointment, cancellationtoken);
            var result = new GetAppointmentsModel()
            {
                Id = cursor.Id,
                Date = cursor.Date,
                StartTime = cursor.StartTime,
                EndTime = cursor.EndTime,
                Summary = cursor.Summary,
                UserEmail = cursor.UserEmail,
                UserName = cursor.UserName
              
            };
             
            SendEmail(new MailData()
            {
                EmailToId = model.UserEmail,
                EmailToName = model.UserName,
                EmailBody = $"{"Hello  "+model.UserName+ "\nYou have an appointment scheduled for "+ model.Date.Date + " at " + model.StartTime.TimeOfDay + "." + " \nKind Regards"  }",
                EmailSubject = model.Summary,
            });
     
            return result;
        }

        public void CancelAppointment(CancelAppointmnentModel model)
        {
            if (model == null)
            {
                throw new Exception(" Appointment not found");
            }
            _appointmentCollection.CancelAppointment(model.Id);
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

        public async Task AppointmentMail(MailData mailData)
        {
            string FilePath = Directory.GetCurrentDirectory() + "/wwwroot/Templates/appointmentTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            
            MailText = MailText.Replace("[username]", mailData.UserName).Replace("[email]", mailData.Email);
            var email = new MimeMessage();
            
            email.Sender = MailboxAddress.Parse(_mailSettings.EmailId);
            email.To.Add(MailboxAddress.Parse(mailData.EmailToName));
            email.Subject = $"Hello {mailData.UserName}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.EmailId, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}