using System;
using Domain.Tickets;

namespace Application.Models.Tickets
{
    public class UpdateTicketModel
    {
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public string urlPath { get; set; }
  
    }
}