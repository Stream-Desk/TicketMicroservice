using System;

namespace Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}