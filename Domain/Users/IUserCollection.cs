using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Tickets;

namespace Domain.Users
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers(User user);
        Task<List<User>> GetUserById(Guid userId);
        Task<List<User>> CreateUser(User user);
        void updateUsers(Guid userId, User user);
        void DeleteUser(User user);
        void DeleteUserById(User user);
        
    }
}