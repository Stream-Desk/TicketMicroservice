using Application.Models.Users;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserCollection _userCollection;

        public UserService(
            IConfiguration configuration,
            IUserCollection userCollection)
        {
            _configuration = configuration;
            _userCollection = userCollection;
        }

        public async Task<GetUserModel> CreateUser(
            AddUserModel model,
            CancellationToken cancellationToken = default)
        {
            // validation: ensure model is not null or empty
            if (model == null)
            {
                throw new Exception("User details are empty");
            }

            // mapping model to the domain entity
            var user = new User
            {
                FirstName = model.FirstName,
                UserName = model.UserName,
                EmailAddress = model.EmailAddress,
                Password = model.Password,
            };

            var result = await _userCollection.CreateUser(user, cancellationToken);

            var response = new GetUserModel
            {
                FirstName = result.FirstName,
                UserName = result.UserName,
                EmailAddress = result.EmailAddress,
                Password = result.Password,
            };

            return response;
        }

        public void DeleteUserById(DeleteUserModel model)
        {
            if (model == null)
            {
                throw new Exception("User Id is empty");
            }
            _userCollection.DeleteUserById(model.Id);
        }

        public async Task<GetUserModel> GetUserById(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("User Id is empty");
            }

            var result = await _userCollection.GetUserById(userId, cancellationToken);

            if (result == null)
            {
                return new GetUserModel();
            }

            var response = new GetUserModel
            {
                FirstName = result.FirstName,
                UserName = result.UserName,
                EmailAddress = result.EmailAddress,
                Password = result.Password,
            };

            return response;
        }

        public async Task<List<GetUserModel>> GetUsers(CancellationToken cancellationToken = default)
        {
            var results = await _userCollection.GetUsers(cancellationToken);

            if (results == null || results.Count < 1)
            {
                return new List<GetUserModel>();
            }

            var response = new List<GetUserModel>();

            // transform from list of users to list of getusermodels
            foreach (var result in results)
            {
                var model = new GetUserModel
                {
                    FirstName = result.FirstName,
                    UserName = result.UserName,
                    EmailAddress = result.EmailAddress,
                    Password = result.Password,
                };

                response.Add(model);
            }
            return response;
        }

        public async Task SeedUsersAsync(CancellationToken cancellationToken = default)
        {
            var users = await GetUsers(cancellationToken);

            if (users != null && users.Count > 0)
            {
                return;
            }

            var usersToSeed = _configuration.GetSection("Users").Get<UserModel[]>();

            if (usersToSeed == null || usersToSeed.Length <= 0)
            {
                return;
            }

            foreach (var userToSeed in usersToSeed)
            {
                await CreateUser(new AddUserModel 
                { 
                    EmailAddress = userToSeed.EmailAddress,
                    FirstName = string.Empty,
                    UserName = userToSeed.UserName,
                    Password = userToSeed.Password
                }, cancellationToken);
            }
        }

        public void UpdateUser(string userId, UpdateUserModel model)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("user Id is empty");
            }

            if (model == null)
            {
                throw new Exception("User details are empty");
            }

            // get current user by id
            var currentUser = _userCollection.GetUserById(userId).Result;

            if (currentUser == null)
            {
                throw new Exception("User does not exist");
            }

            currentUser.FirstName = model.FirstName;
            currentUser.UserName = model.UserName;
            currentUser.EmailAddress = model.EmailAddress;
            currentUser.Password = model.Password;
            _userCollection.UpdateUser(userId, currentUser);
        }
    }
}
