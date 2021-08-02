using System;

namespace Domain.Tickets
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public Priority Priority  { get; set; }
    }

    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}