using System.Collections.Generic;
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
        public async Task<Comment> CreateComment(Comment comment, CancellationToken cancellationToken = default)
        {
            await _commentCollection.InsertOneAsync(comment);
            return comment;
        }
        
        public async Task<List<Comment>> GetComments(CancellationToken cancellationToken = default)
        {
            var cursor = await _commentCollection.FindAsync(c => true);
            var comments = await cursor.ToListAsync(cancellationToken);
            return comments;
        }

        public async Task<Comment> GetCommentById(string commentId, CancellationToken cancellationToken = default)
        {
            var cursor = await _commentCollection.FindAsync(c => c.Id == commentId);
            var comment = await cursor.FirstOrDefaultAsync(cancellationToken);
            return comment;
        }
    }
}