using System;
using Microsoft.VisualBasic;

namespace Domain.Appointments
{
    public class Appointment
    {
        public string AppointmentId { get; set; }
        public DateTime BookingDate { get; set; } 
        public DateTime AppointmentDate { get; set; }
        public DateAndTime AppointmentTime { get; set; }
        public string UserName { get; set; }
    }
}