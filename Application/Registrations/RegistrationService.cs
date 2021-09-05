using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Registrations;
using Domain.Registrations;

namespace Application.Registrations
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationCollection _registrationCollection;

        public RegistrationService(IRegistrationCollection registrationCollection)
        {
            _registrationCollection = registrationCollection;
        }
        public async Task<List<GetRegistrationModel>> GetRegistrations(CancellationToken cancellationToken = default)
        {
            var searchResults = await _registrationCollection.GetRegistrations(cancellationToken);
            if (searchResults == null || searchResults.Count < 1)
            {
                return new List<GetRegistrationModel>();
            }

            var result = new List<GetRegistrationModel>();

            foreach (var searchResult in searchResults)
            {
                var model = new GetRegistrationModel
                {
                    Id = searchResult.Id,
                    FirstName = searchResult.FirstName,
                    LastName = searchResult.LastName,
                    UserName = searchResult.UserName,
                    Role = searchResult.Role,
                    Email = searchResult.Email,
                    TelNumber = searchResult.TelNumber,
                    SubmitDate = searchResult.SubmitDate,
                   
                };
                result.Add(model);
            }
            return result;
        }

        public async Task<GetRegistrationModel> GetRegistrationById(string registrationId, CancellationToken cancellationToken = default)
        {
            // validate
            if (string.IsNullOrWhiteSpace(registrationId))
            {
                throw new Exception("Registration not Found");
            }

            var search = await _registrationCollection.GetRegistrationById(registrationId, cancellationToken);
            if (search == null)
            {
                return new GetRegistrationModel();
            }

            var result = new GetRegistrationModel
            {
                Id = search.Id,
                FirstName = search.FirstName,
                LastName = search.LastName,
                UserName = search.UserName,
                Role = search.Role,
                Email = search.Email,
                TelNumber = search.TelNumber,
                SubmitDate = search.SubmitDate,
            };
            return result;
        }

        public async Task<GetRegistrationModel> CreateRegistration(AddRegistrationModel model, CancellationToken cancellationToken = default)
        {
            // validate model

            if (model == null)
            {
                throw new Exception("Registration details empty");
            }

            // Map model to domain Entity
            var registration = new Registration
            {
                
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Role = model.Role,
                Email = model.Email,
                TelNumber = model.TelNumber,
                SubmitDate = model.SubmitDate,
            };

            var search = await _registrationCollection.CreateRegistration(registration, cancellationToken);
            var result = new GetRegistrationModel
            {
                FirstName = search.FirstName,
                LastName = search.LastName,
                UserName = search.UserName,
                Role = search.Role,
                Email = search.Email,
                TelNumber = search.TelNumber,
                SubmitDate = search.SubmitDate,
            };
            return result;
        }

        public void UpdateRegistration (string registrationId, UpdateRegistrationModel model)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(registrationId))
            {
                throw new Exception("Registration Id does not exist");
            }

            if (model == null)
            {
                throw new Exception("Failed to find registration");
            }

            // get ticket by Id
            var registration = _registrationCollection.GetRegistrationById(registrationId).Result;

            if (registration == null)
            {
                throw new Exception("Registration not found");
            }

            registration.FirstName = model.FirstName;
            registration.LastName = model.LastName;
            registration.UserName = model.UserName;
            registration.Role = model.Role;
            registration.Email = model.Email;
            registration.TelNumber = model.TelNumber;
            registration.SubmitDate = model.SubmitDate;

            _registrationCollection.UpdateRegistration(registrationId, registration);
        }
        public void DeleteRegistrationById(DeleteRegistrationModel model)
        {
            // validation
            if (model == null)
            {
                throw new Exception("Registration not found");
            }
            _registrationCollection.DeleteRegistrationById(model.Id);
        }
    }
}