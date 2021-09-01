using System;
using Domain.Tickets;
using Domain.Users;

namespace Application.Models.Tickets
{
    public class AddTicketModel
    {
        public string Summary { get; set; }
        public string TicketNumber { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public string Attachment{ get; set; }
   
    }
}