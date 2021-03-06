using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Tickets;

namespace Application.Tickets
{
    public interface ITicketService
    {
        // Banks BO Ticket List
        Task<List<GetTicketModel>> GetTicketsWithSoftDeleteFalse(CancellationToken cancellationToken = default);
        // Laboremus Ticket List
        Task<List<GetTicketModel>> GetTickets(CancellationToken cancellationToken = default);
        Task<GetTicketModel> GetTicketById(string ticketId, CancellationToken cancellationToken = default);
        // Get ticket By Id ( Laboremus)
        Task<GetTicketModel> GetTicketByIdLaboremus(string ticketId, CancellationToken cancellationToken = default);
        Task<GetTicketModel> CreateTicket(AddTicketModel model, CancellationToken cancellationToken = default);
        void UpdateTicket(string ticketId, UpdateTicketModel model);
        void AssignTicket(UpdateTicketModel model);
        void UpdateTicketStatus(string ticketId, UpdateTicketModel model);
        void CloseTicket(UpdateTicketModel model);
        void DeleteTicketById(DeleteTicketModel model);
        void IsSoftDeleted(DeleteTicketModel model);
    }
}