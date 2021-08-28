using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Drafts
{
   public interface IDraftCollection
    {
        Task<List<Draft>> GetDrafts(CancellationToken cancellationToken = default);
        Task<Draft> GetDraftById(string draftId, CancellationToken cancellationToken = default);
        Task<Draft> CreateDraft(Draft draft, CancellationToken cancellationToken = default);
        void UpdateDraft(string draftId, Draft draft);
        void DeleteDraftById(string draftId);
    }
}
