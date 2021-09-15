using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Appointments;
using Domain.Appointments;

namespace Application.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentCollection _appointmentCollection;

        public AppointmentService(IAppointmentCollection appointmentCollection)
        {
            _appointmentCollection = appointmentCollection;
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
    }
}