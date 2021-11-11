using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Statuses;

namespace Application.Statuses
{
    public interface IStatusService
    {
        Task<GetStatusModel> CreateStatus(AddStatusModel model, CancellationToken cancellationToken = default);
        Task<GetStatusModel> GetStatusById(string statusId, CancellationToken cancellationToken = default);
        Task<List<GetStatusModel>> GetAllStatuses(CancellationToken cancellationToken = default);
    }
}