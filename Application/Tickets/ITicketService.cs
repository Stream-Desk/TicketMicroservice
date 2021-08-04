using System.Threading;
using Application.Models.Tickets;

namespace Application.Tickets
{
    public interface ITicketService
    {
        Task<List<GetTicketModel>> GetTickets(CancellationToken cancellationToken = default);
        Task<GetTicketModel> GetTicketById(string ticketId, CancellationToken cancellationToken = default);
        Task<GetTicketModel> CreateTicket(AddTicketModel model, CancellationToken cancellationToken = default);
        void UpdateTicket(string ticketId, UpdateTicketModel model);
        void DeleteTicketById(DeleteTicketModel model);
    }
}