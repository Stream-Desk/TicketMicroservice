using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Tickets;
using Domain.Tickets;
using MongoDB.Driver;

namespace Application.Tickets
{
    public interface ITicketService
    {
        // Banks BO Ticket List
        Task<List<GetTicketModel>> GetTicketsWithSoftDeleteFalse(CancellationToken cancellationToken = default);
        // Laboremus Ticket List
        Task<List<GetTicketModel>> GetTickets(CancellationToken cancellationToken = default);
        Task<GetTicketModel> GetTicketById(string ticketId, CancellationToken cancellationToken = default);
        Task<GetTicketModel> CreateTicket(AddTicketModel model, CancellationToken cancellationToken = default);
        void UpdateTicket(string ticketId, UpdateTicketModel model);
        void DeleteTicketById(DeleteTicketModel model);
        void IsSoftDeleted(string ticketId, DeleteTicketModel model);
        Task<List<GetTicketModel>> SearchTickets(string searchTerm, CancellationToken cancellationToken = default);
        Task<List<GetTicketModel>> SearchResult(string q, int page);
        Task<List<GetTicketModel>> Pagination( int page, CancellationToken cancellationToken = default);

    }
}