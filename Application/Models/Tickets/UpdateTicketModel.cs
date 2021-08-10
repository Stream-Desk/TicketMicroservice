using System;
using System.Net.Mime;
using Domain.Tickets;

namespace Application.Models.Tickets
{
    public class UpdateTicketModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public Priority Priority  { get; set; }
        public DateTime SubmitDate { get; set; }
        public  string Attachment { get; set; }
  
    }
}