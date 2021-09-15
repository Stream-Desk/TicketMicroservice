using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Appointments
{
    public interface IAppointmentCollection
    {
        Task<List<Appointment>> GetAppointments(CancellationToken cancellationToken = default);
        Task<Appointment> GetAppointmentById(string appointmentId, CancellationToken cancellationtoken = default);
        Task<Appointment> CreateAppointment(Appointment appointment, CancellationToken cancellationtoken = default);
        void CancelAppointment(string appointmentId);
    }
}