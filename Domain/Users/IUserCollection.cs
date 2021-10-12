using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Users
{
    public interface IUserCollection
    {
        Task<List<User>> GetUsers(CancellationToken cancellationToken = default);
        Task<User> GetUserById(string userId,CancellationToken cancellationToken = default);
        Task<User> CreateUser(User user,CancellationToken cancellationToken = default);
        void UpdateUser(string userId, User user);
        void DeleteUserById(string userId);
        Task<User> Authenticate(string UserName, string password, CancellationToken cancellationToken = default);

    }
}