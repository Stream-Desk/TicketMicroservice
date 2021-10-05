using Application.Models.Users;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public interface IUserService
    {
        Task<List<GetUserModel>> GetUsers(CancellationToken cancellationToken = default);

        Task<GetUserModel> GetUserById(string userId, CancellationToken cancellationToken = default);

        Task<GetUserModel> CreateUser(AddUserModel model, CancellationToken cancellationToken = default);

        void UpdateUser(string userId, UpdateUserModel model);

        void DeleteUserById(DeleteUserModel model);
        Task Authenticate(string userName, string password);

        Task SeedUsersAsync(CancellationToken cancellationToken = default);
       
    }
}

