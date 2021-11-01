using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Drafts;
using Domain.Drafts;

namespace Application.Drafts
{
    public class DraftsService : IDraftService
    {
        private readonly IDraftCollection _draftCollection;

        public DraftsService(IDraftCollection draftCollection)
        {
            _draftCollection = draftCollection;
        }
        public async Task<List<GetDraftModel>> GetDrafts(CancellationToken cancellationToken = default)
        {
            var searchResults = await _draftCollection.GetDrafts(cancellationToken);
            if (searchResults == null || searchResults.Count < 1)
            {
                return new List<GetDraftModel>();
            }

            var result = new List<GetDraftModel>();

            foreach (var searchResult in searchResults)
            {
                var model = new GetDraftModel
                {
                    Id = searchResult.Id,
                    Description = searchResult.Description,
                    Summary = searchResult.Summary,
                    Category = searchResult.Category,
                    Priority = searchResult.Priority,
                    Status = searchResult.Status,
                    SubmitDate = searchResult.SubmitDate,
                };
                result.Add(model);
            }
            return result;
        }

        public async Task<GetDraftModel> GetDraftById(string draftId, CancellationToken cancellationToken = default)
        {
            // validate
            if (string.IsNullOrWhiteSpace(draftId))
            {
                throw new Exception("Draft not Found");
            }

            var search = await _draftCollection.GetDraftById(draftId, cancellationToken);
            if (search == null)
            {
                return new GetDraftModel();
            }

            var result = new GetDraftModel
            {
                Id = search.Id,
                Description = search.Description,
                Summary = search.Summary,
                Category = search.Category,
                Priority = search.Priority,
                SubmitDate = search.SubmitDate,
                Status = search.Status,
                FileUrls = search.FileUrls
            };
            return result;
        }

        public async Task<GetDraftModel> CreateDraft(AddDraftModel model, CancellationToken cancellationToken = default)
        {
            // validate model

            if (model == null)
            {
                throw new Exception("Draft details empty");
            }

            // Map model to domain Entity
            var draft = new Draft
            {
                Description = model.Description,
                Summary = model.Summary,
                Category = model.Category,
                Priority = model.Priority,
                SubmitDate = DateTime.Now,
                Status = model.Status,
                FileUrls = model.FileUrls
            };

            var search = await _draftCollection.CreateDraft(draft, cancellationToken);
            var result = new GetDraftModel
            {
                Description = search.Description,
                Priority = search.Priority,
                Summary = search.Summary,
                Category = search.Category,
                SubmitDate = DateTime.Now,
                Status = search.Status,
                FileUrls = search.FileUrls
            };
            return result;
        }

        public void UpdateDraft(string draftId, UpdateDraftModel model)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(draftId))
            {
                throw new Exception("Draft Id does not exist");
            }

            if (model == null)
            {
                throw new Exception("Failed to find Draft");
            }

            // get Draft by Id
            var draft = _draftCollection.GetDraftById(draftId).Result;

            if (draft == null)
            {
                throw new Exception("Draft not found");
            }

            draft.Summary = model.Summary;
            draft.Description = model.Description;
            draft.Category = model.Category;
            draft.Priority = model.Priority;
            draft.SubmitDate = DateTime.Now;
            draft.Status = model.Status;
            draft.FileUrls = model.FileUrls;

            _draftCollection.UpdateDraft(draftId, draft);
        }
        public void DeleteDraftById(DeleteDraftModel model)
        {
            // validation
            if (model == null)
            {
                throw new Exception("Draft not found");
            }
            _draftCollection.DeleteDraftById(model.Id);
        }
    }
}
