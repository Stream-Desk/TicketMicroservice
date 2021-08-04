using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Tickets;

namespace Domain.Users
{
    public interface IUserCollection
    {
        Task<List<User>> GetUsers(CancellationToken cancellationToken = default);
        Task<User> GetUserById(string userId,CancellationToken cancellationToken = default);
        Task<User> CreateUser(User user,CancellationToken cancellationToken = default);
        void UpdateUser(string userId, User user);
       
        void DeleteUserById(string userId);
        
    }
}