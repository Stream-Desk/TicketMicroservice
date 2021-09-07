using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Registrations
{
    public interface IRegistrationCollection
    {

        Task<List<Registration>> GetRegistrations(CancellationToken cancellationToken = default);
        Task<Registration> GetRegistrationById(string registrationId, CancellationToken cancellationToken = default);
        Task<Registration> CreateRegistration(Registration registration, CancellationToken cancellationToken = default);
        void UpdateRegistration(string registrationId, Registration registration);
        void DeleteRegistrationById(string registrationId);


    }
}