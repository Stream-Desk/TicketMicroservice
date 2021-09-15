using System;
using Microsoft.VisualBasic;

namespace Application.Models.Appointments
{
    public class GetAppointmentsModel
    {
        public string AppointmentId { get; set; }
        public DateTime BookingDate { get; set; } 
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string UserId { get; set; }
    }
}