using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Registrations;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database.Collections
{
    public class RegistrationsCollection : IRegistrationCollection
    {
        private IMongoCollection<Registration> _registrationCollection;



        public RegistrationsCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var registrationsCollectionName = configuration.GetValue<string>("MongoDb:RegistrationCollection");

            _registrationCollection = database.GetCollection<Registration>(registrationsCollectionName);

        }


        public async Task<List<Registration>> GetRegistrations(CancellationToken cancellationToken = default)
        {
            var cursor = await _registrationCollection.FindAsync(a => true);
            var registration = await cursor.ToListAsync(cancellationToken);
            return registration;
        }

        public async Task<Registration> GetRegistrationById(string registrationId, CancellationToken cancellationToken = default)
        {
            var cursor = await _registrationCollection.FindAsync(a => a.Id == registrationId);
            var registration = await cursor.FirstOrDefaultAsync(cancellationToken);
            return registration;
        }

        public async Task<Registration> CreateRegistration(Registration registration, CancellationToken cancellationToken = default)
        {
            await _registrationCollection.InsertOneAsync(registration, cancellationToken: cancellationToken);
            return registration;
        }
       

        public void UpdateRegistration(string registrationId, Registration registration)
        {
            _registrationCollection.ReplaceOne(a => a.Id == registrationId, registration);
        }

        public void DeleteRegistrationById(string registrationId)
        {
            _registrationCollection.DeleteOne(a => a.Id == registrationId);
        }

        
    }

}