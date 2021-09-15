using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Appointments;
using Domain.Appointments;

namespace Application.Appointments
{
    public interface IAppointmentService
    {
        Task<List<GetAppointmentsModel>> GetAppointments(CancellationToken cancellationToken = default);
        Task<GetAppointmentsModel> GetAppointmentById(string appointmentId, CancellationToken cancellationtoken = default);
        Task<GetAppointmentsModel> CreateAppointment(AddAppointmentModel model, CancellationToken cancellationtoken = default);
        void CancelAppointment(CancelAppointmnentModel model);
    }
}