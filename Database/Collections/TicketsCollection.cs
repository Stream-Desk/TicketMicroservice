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
        private IMongoCollection<Ticket> _ticketCollection;
        private object ticketNumber;

        public TicketsCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var ticketsCollectionName = configuration.GetValue<string>("MongoDb:TicketCollection");
            _ticketCollection = database.GetCollection<Ticket>(ticketsCollectionName);
        
        }

        // Banks BO List
        public async Task<List<Ticket>> GetTicketsWithSoftDeleteFalse(CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketCollection.FindAsync(t => t.IsDeleted == false);
            var ticket = await cursor.ToListAsync(cancellationToken);
            return ticket;
        }

        // Laboremus Ticket List
        public async Task<List<Ticket>> GetTickets(CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketCollection.FindAsync(t => true);
            var ticket = await cursor.ToListAsync();
            return ticket;
        }

        public async Task<Ticket> GetTicketById(string ticketId, CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketCollection.FindAsync(t => t.Id == ticketId);
            var ticket = await cursor.FirstOrDefaultAsync(cancellationToken);
            return ticket;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket, CancellationToken cancellationToken = default)
        {
            await _ticketCollection.InsertOneAsync(ticket);
            return ticket;
        }

       

        public async Task<List<Ticket>> SearchTicket(string searchTerm, CancellationToken cancellationToken = default)
        {

            var keys = Builders<Ticket>.IndexKeys.Text(t => t.Summary);
             _ticketCollection.Indexes.CreateOne(keys);
            var filter = Builders<Ticket>.Filter.Text(searchTerm);
            var sortDefinition = Builders<Ticket>.Sort.Descending(a => a.SubmitDate);
            var result =  _ticketCollection.Find(filter).Sort(sortDefinition).ToList(cancellationToken);
            return result;
        }

        public void UpdateTicket(string ticketId, Ticket ticket)
        {
            _ticketCollection.ReplaceOne(t => t.Id == ticketId, ticket);
        }
        
        public void DeleteTicketById(string ticketId)
        {
            _ticketCollection.DeleteOne(t => t.Id == ticketId);
        }

        public void IsSoftDeleted(string ticketId, Ticket ticket)
        {
            _ticketCollection.ReplaceOne(t => t.Id == ticketId,ticket);
        }
    }
}
