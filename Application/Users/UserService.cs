using Application.Models.Users;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserCollection _userCollection;

        public UserService(IUserCollection userCollection)
        {
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
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,

            };

            var result = await _userCollection.CreateUser(user, cancellationToken);

            var response = new GetUserModel
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                EmailAddress = result.EmailAddress,
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
                LastName = result.LastName,
                EmailAddress = result.EmailAddress,
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
                    LastName = result.LastName,
                    EmailAddress = result.EmailAddress,
                };

                response.Add(model);
            }
            return response;
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
            currentUser.LastName = model.LastName;
            currentUser.EmailAddress = model.EmailAddress;
            _userCollection.UpdateUser(userId, currentUser);
        }
    }
}
