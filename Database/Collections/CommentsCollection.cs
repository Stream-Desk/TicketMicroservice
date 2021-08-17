using System.Threading;
using System.Threading.Tasks;
using Domain.Comments;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database.Collections
{
    public class CommentsCollection : ICommentsCollection
    {
        private IMongoCollection<Comment> _commentCollection;

        public CommentsCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var commentCollectionName = configuration.GetValue<string>("MongoDb:CommentCollection");
            _commentCollection = database.GetCollection<Comment>(commentCollectionName);
        }
        public Task<Comment> CreateComment(Comment comment, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<Comment> GetComment(string commentId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}