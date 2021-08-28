using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Drafts;


namespace Application.Drafts
{
    public interface IDraftService
    {
        Task<List<GetDraftModel>> GetDrafts(CancellationToken cancellationToken = default);
        Task<GetDraftModel> GetDraftById(string draftId, CancellationToken cancellationToken = default);
        Task<GetDraftModel> CreateDraft(AddDraftModel model, CancellationToken cancellationToken = default);
        void UpdateDraft(string draftId, UpdateDraftModel model);
        void DeleteDraftById(DeleteDraftModel model);
    }
}
