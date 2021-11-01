using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Drafts;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database.Collections
{
    public class DraftsCollection : IDraftCollection
    {
        private IMongoCollection<Draft> _draftCollection;
        

        public DraftsCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var draftsCollectionName = configuration.GetValue<string>("MongoDb:DraftCollection");

            _draftCollection = database.GetCollection<Draft>(draftsCollectionName);
        }

        public async Task<List<Draft>> GetDrafts(CancellationToken cancellationToken = default)
        {
            var cursor = _draftCollection.Find(a => true)
                .SortByDescending(x=> x.Id);
            var draft = await cursor.ToListAsync(cancellationToken);
            return draft;
        }

        public async Task<Draft> GetDraftById(string draftId, CancellationToken cancellationToken = default)
        {
            var cursor = await _draftCollection.FindAsync(a => a.Id == draftId);
            var draft = await cursor.FirstOrDefaultAsync(cancellationToken);
            return draft;
        }

        public async Task<Draft> CreateDraft(Draft draft, CancellationToken cancellationToken = default)
        {
            await _draftCollection.InsertOneAsync(draft);
            return draft;
        }

        public void UpdateDraft(string draftId, Draft draft)
        {
            _draftCollection.ReplaceOne(a => a.Id == draftId, draft);
        }

        public void DeleteDraftById(string draftId)
        {
            _draftCollection.DeleteOne(a => a.Id == draftId);
        }
    }
    
}
   