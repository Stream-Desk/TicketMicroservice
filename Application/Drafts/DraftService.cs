using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Drafts;
using Domain.Drafts;
using Domain.Tickets;
using Microsoft.Extensions.DependencyInjection;
using Category = Domain.Drafts.Category;

namespace Application.Drafts
{
    public class DraftsService : IDraftService
    {
        private readonly IDraftCollection _draftCollection;
        private readonly IServiceScopeFactory _scopeFactory;

        public DraftsService(
            IDraftCollection draftCollection,
            IServiceScopeFactory scopeFactory)
        {
            _draftCollection = draftCollection;
            _scopeFactory = scopeFactory;
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
                    Name = searchResult.Name,
                    Summary = searchResult.Summary,
                    Priority = searchResult.Priority,
                    Status = searchResult.Status,
                    Category = (Domain.Tickets.Category)searchResult.Category,
                    SubmitDate = searchResult.SubmitDate,
                    IsModified = searchResult.IsModified, 
                    
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
                Name =search.Name,
                Summary = search.Summary,
                Category = (Domain.Tickets.Category)search.Category,
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
                Name = model.Name,
                Summary = model.Summary,
                Category = (Domain.Drafts.Category)model.Category,
                Priority = model.Priority,
                SubmitDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"),
                Status = Status.Open,
                FileUrls = model.FileUrls
            };

            var search = await _draftCollection.CreateDraft(draft, cancellationToken);
            var result = new GetDraftModel
            {

                Id = search.Id,
                Description = search.Description,
                Name = search.Name,
                Priority = search.Priority,
                Summary = search.Summary,
                Category = (Domain.Tickets.Category)search.Category,
                SubmitDate = search.SubmitDate,
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

            // Category to March Priority
                
            switch (draft.Category)
            {
                case Category.Bug:
                    model.Priority = Priority.High;
                    break;
                case Category.Login:
                    model.Priority = Priority.High;
                    break;
                case Category.Uploads:
                    model.Priority = Priority.Medium;
                    break;
                case Category.Other:
                    model.Priority = Priority.Low;
                    break;
                case Category.FreezingScreen:
                    model.Priority = Priority.High;
                    break;  
                default:
                    model.Priority = Priority.Low;
                    break;
            }
            
            draft.Summary = model.Summary;
            draft.Name = model.Name;
            draft.Description = model.Description;
            draft.Category = (Category)model.Category;
            draft.Priority = model.Priority;
            draft.ModifiedAt = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
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
