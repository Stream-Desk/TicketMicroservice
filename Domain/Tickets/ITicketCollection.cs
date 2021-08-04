using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Tickets
{
    public interface ITicketCollection
    {
        Task<List<Ticket>> GetTickets(CancellationToken cancellationToken = default);
        Task<Ticket> GetTicketById(string ticketId, CancellationToken cancellationToken = default);
        Task<Ticket> CreateTicket(Ticket ticket, CancellationToken cancellationToken = default);
        void UpdateTicket(string ticketId, Ticket ticket);
        void DeleteTicketById(string ticketId);

    }
}