using System;
using System.Collections.Generic;
using Domain.Tickets;

namespace Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}