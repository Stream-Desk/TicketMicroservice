using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Tickets;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database.Collections
{
    public class TicketsCollection : ITicketCollection
    {
        private IMongoCollection<Ticket> _ticketsCollection;

        public TicketsCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var ticketsCollectionName = configuration.GetValue<string>("MongoDb:TicketCollection");

            _ticketsCollection = database.GetCollection<Ticket>(ticketsCollectionName);
        }
       
        public async Task<List<Ticket>> GetTickets(CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketsCollection.FindAsync(a => true);
        
            var ticket = await cursor.ToListAsync(cancellationToken);
        
            return ticket;
        }

        public async Task<Ticket> GetTicketById(string ticketId, CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketsCollection.FindAsync(a => a.Id == ticketId);
            
            var ticket = await cursor.FirstOrDefaultAsync(cancellationToken);
            
            return ticket;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket, CancellationToken cancellationToken = default)
        {
            await _ticketsCollection.InsertOneAsync(ticket);
            return ticket;
        }

        public void UpdateTicket(string ticketId, Ticket ticket)
        {
            _ticketsCollection.ReplaceOne(a => a.Id == ticketId, ticket);
        }
        
        public void DeleteTicketById(string ticketId)
        {
            _ticketsCollection.DeleteOne(a => a.Id == ticketId);
        }
    }
    
}
