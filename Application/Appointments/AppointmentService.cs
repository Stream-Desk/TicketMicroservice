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
        public Task<List<GetAppointmentsModel>> GetAppointments(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<GetAppointmentsModel> GetAppointmentById(string appointmentId, CancellationToken cancellationtoken = default)
        {
            throw new System.NotImplementedException();
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