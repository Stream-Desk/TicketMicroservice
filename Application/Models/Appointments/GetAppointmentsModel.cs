using System;

namespace Application.Models.Appointments
{
    public class GetAppointmentsModel
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
        public string Summary { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}