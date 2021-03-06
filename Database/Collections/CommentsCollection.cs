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
        public async Task<Comment> CreateComment(Comment comment)
        {
            await _commentCollection.InsertOneAsync(comment);
            return comment;
        }
        
        public async Task<List<Comment>> GetComments(CancellationToken cancellationToken = default)
        {
            var cursor = _commentCollection
                .Find(c => true)
                .SortByDescending(c => c.Id);
            var comments = await cursor.ToListAsync(cancellationToken);
            return comments;
        }

        public async Task<Comment> GetCommentById(string commentId, CancellationToken cancellationToken = default)
        {
            var cursor = await _commentCollection.FindAsync(c => c.Id == commentId);
            var comment = await cursor.FirstOrDefaultAsync(cancellationToken);
            return comment;
        }

        public void UpdateComments(string commentId, Comment comment)
        {
            _commentCollection.ReplaceOne(c => c.Id == commentId,comment);
            
        }

        public void DeleteCommentById(string commentId)
        {
            _commentCollection.DeleteOne(c => c.Id == commentId);
        }

        public async Task<List<Comment>> GetCommentByTicketIdAsync(string ticketId, CancellationToken cancellationToken = default)
        {
            // order the comments by TimeStamp desc
            var cursor = _commentCollection
                .Find(c => c.TicketId == ticketId)
                .SortByDescending(x => x.Id);
            var comments = await cursor.ToListAsync(cancellationToken);
            return comments;
        }
    }
}