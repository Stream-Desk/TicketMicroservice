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
            var cursor = _ticketCollection.Find(t => t.IsDeleted == false)
                .SortByDescending(t => t.Id);
            var ticket = await cursor.ToListAsync(cancellationToken);

            return ticket;
        }

        // Laboremus Ticket List
        public async Task<List<Ticket>> GetTickets(CancellationToken cancellationToken = default)
        {
            var cursor =  _ticketCollection.Find(a => true)
                .SortByDescending(t => t.Id);
            var ticket = await cursor.ToListAsync(cancellationToken);
            return ticket;
        }

        public async Task<Ticket> GetTicketById(string ticketId, CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketCollection.FindAsync(t => t.Id == ticketId);
            var ticket = await cursor.FirstOrDefaultAsync(cancellationToken);
            return ticket;
        }

        public async Task<Ticket> GetTicketByIdLaboremus(string ticketId, CancellationToken cancellationToken = default)
        {
            var cursor = await _ticketCollection.FindAsync(x => x.Id == ticketId);
            var ticket = await cursor.FirstOrDefaultAsync(cancellationToken);
            return ticket;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket, CancellationToken cancellationToken = default)
        {
            await _ticketCollection.InsertOneAsync(ticket);
            return ticket;
        }

        public void UpdateTicket(string ticketId, Ticket ticket)
        {
            _ticketCollection.ReplaceOne(t => t.Id == ticketId, ticket);
        }

        public void AssignTicket(string ticketId, Ticket ticket)
        {
            _ticketCollection.ReplaceOne(t => t.Id == ticketId, ticket);
        }

        public void UpdateTicketStatus(string ticketId, Ticket ticket)
        {
            _ticketCollection.ReplaceOne(t => t.Id == ticketId, ticket);
        }

        public void CloseTicket(string ticketId, Ticket ticket)
        {
            _ticketCollection.ReplaceOne(t => t.Id == ticketId, ticket);
        }

        public void DeleteTicketById(string ticketId)
        {
            _ticketCollection.DeleteOne(t => t.Id == ticketId);
        }
        
        public void IsSoftDeleted(string ticketId, Ticket ticket)
        {
            _ticketCollection.ReplaceOne(t => t.Id == ticketId, ticket);
        }
    }
}
