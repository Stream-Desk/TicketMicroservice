using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Statuses
{
    public interface IStatusCollection
    {
        Task<Status> CreateTicketStatus(Status status, CancellationToken cancellationToken = default);
        Task<Status> GetTicketStatus(string statusId, CancellationToken cancellationToken = default);
        Task<List<Status>> GetAllTicketStatuses(CancellationToken cancellationToken = default);
    }
}