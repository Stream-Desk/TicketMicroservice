using Domain.Tickets;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Colections
{
    public class TicketsCollection : ITicketsCollection
    {
        private IMongoCollection<Tickets> _ticketsCollection;

        public TicketsCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("mongodb+srv://CathyAkoth:<Malaika221188!>@stream-desk.pellu.mongodb.net/TicketsDB?retryWrites=true&w=majority");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var TicketsDB = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(TicketsDB);
            var ticketsCollectionName = configuration.GetValue<string>("MongoDb:TicketsCollection");

            _ticketsCollection = database.GetCollection<Tickets>(ticketsCollectionName);
        }

        public async Task<Tickets> CreateTickets(Tickets tickets, CancellationToken cancellationToken = default)
        {
            await _ticketsCollection.InsertOneAsync(tickets);

            return tickets;
        }

        public void DeleteTicketsById(string ticketsId)
        {
            _ticketsCollection.DeleteOne(a => a.Id == ticketsId);
        }

        public async Task<Tickets> GetticketsById(string ticketsId, CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketsCollection.FindAsync(a => a.Id == ticketsId);

            var tickets = await cursor.FirstOrDefaultAsync(cancellationToken);

            return tickets;
        }

        public async Task<List<Tickets>> GetTickets(CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketsCollection.FindAsync(a => true);

            var tickets = await cursor.ToListAsync(cancellationToken);

            return tickets;
        }

        public void UpdateTickets(string ticketsId, Tickets tickets)
        {
            _ticketsCollection.ReplaceOne(a => a.Id == ticketsId, tickets);
        }
    }
}
