using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Domain.Tickets
{
    public interface ITicketCollection
    {
        // Banks BO List of Tickets
        Task<List<Ticket>> GetTicketsWithSoftDeleteFalse(CancellationToken cancellationToken = default);
        // Laboremus List of Tickets
        Task<List<Ticket>> GetTickets(CancellationToken cancellationToken = default);
        Task<Ticket> GetTicketById(string ticketId, CancellationToken cancellationToken = default);
        Task<Ticket> CreateTicket(Ticket ticket, CancellationToken cancellationToken = default);
        Task<List<Ticket>> SearchTicket(string searchTerm, CancellationToken cancellationToken = default);
        void UpdateTicket(string ticketId, Ticket ticket);
        void DeleteTicketById(string ticketId);
        void IsSoftDeleted(string ticketId, Ticket ticket);
        // Paging
        Task<List<Ticket>> SearchResult(string q, int page);
        Task<List<Ticket>> Pagination(int page, CancellationToken cancellationToken = default);
    }
}