using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Registrations;


namespace Application.Registrations
{
    public interface IRegistrationService
    {
        Task<List<GetRegistrationModel>> GetRegistrations(CancellationToken cancellationToken = default);
        Task<GetRegistrationModel> GetRegistrationById(string registrationId, CancellationToken cancellationToken = default);
        Task<GetRegistrationModel> CreateRegistration(AddRegistrationModel model, CancellationToken cancellationToken = default);
        void UpdateRegistration(string registrationId, UpdateRegistrationModel model);
        void DeleteRegistrationById(DeleteRegistrationModel model);

    }
}