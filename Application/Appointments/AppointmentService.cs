using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Appointments;
using Application.Services;
using Application.Settings;
using Domain.Appointments;
using MailKit.Net.Smtp;
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
                    AppointmentId = searchResult.AppointmentId,
                    AppointmentDate = searchResult.AppointmentDate,
                    AppointmentTime = searchResult.AppointmentTime,
                    BookingDate = searchResult.BookingDate,
                    UserId = searchResult.UserId
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
                AppointmentId = cursor.AppointmentId,
                AppointmentDate = cursor.AppointmentDate,
                AppointmentTime = cursor.AppointmentTime,
                BookingDate = cursor.BookingDate,
                UserId = cursor.UserId
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
                AppointmentDate = model.AppointmentDate,
                BookingDate = model.BookingDate,
                UserId = model.UserId,
                AppointmentTime = model.AppointmentTime
            };

            var cursor = await _appointmentCollection.CreateAppointment(appointment, cancellationtoken);
            var result = new GetAppointmentsModel()
            {
                AppointmentId = cursor.AppointmentId,
                AppointmentDate = cursor.AppointmentDate,
                AppointmentTime = cursor.AppointmentTime,
                BookingDate = cursor.BookingDate,
                UserId = cursor.UserId
            };
            SendEmail(new MailData());
            return result;
        }

        public void CancelAppointment(CancelAppointmnentModel model)
        {
            if (model == null)
            {
                throw new Exception(" Appointment not found");
            }
            _appointmentCollection.CancelAppointment(model.AppointmentId);
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

                emailMessage.Subject = "Appointment confirmed";

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.EmailBody;
                // emailMessage.Body = emailBodyBuilder.ToMessageBody();
                emailMessage.Body = new TextPart("plain")
                {
                    Text = "Appointment Confirmed"
                };

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
                emailClient.Authenticate(_mailSettings.EmailId, _mailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error Occured");
            }
        }
    }
}