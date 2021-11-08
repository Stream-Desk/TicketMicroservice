using System.Collections.Generic;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Domain.Statuses;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database.Collections
{
    public class StatusCollection : IStatusCollection
    {
        private IMongoCollection<Status> _statusCollection;
        public StatusCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var databaseName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(databaseName);
            var statusCollectionName = configuration.GetValue<string>("MongoDb:StatusCollection");

            _statusCollection = database.GetCollection<Status>(statusCollectionName);
        }
        
        public async Task<Status> CreateTicketStatus(Status status, CancellationToken cancellationToken = default)
        {
           await _statusCollection.InsertOneAsync(status, cancellationToken);
           return status;
        }

        public async Task<Status> GetTicketStatus(string statusId, CancellationToken cancellationToken = default)
        {
            var cursor = await _statusCollection.FindAsync(s => s.Id == statusId);
            var status = await cursor.FirstOrDefaultAsync(cancellationToken);
            return status;
        }

        public async Task<List<Status>> GetAllTicketStatuses(CancellationToken cancellationToken = default)
        {
            var cursor = await _statusCollection.FindAsync(x => true);
            var statuses = await cursor.ToListAsync(cancellationToken);
            return statuses;
        }
    }
}