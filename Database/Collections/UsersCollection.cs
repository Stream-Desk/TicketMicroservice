using Domain.Users;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Collections
{
    public class UsersCollection : IUserCollection
    {
        private IMongoCollection<User> _userCollection;

        public UsersCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var usersCollectionName = configuration.GetValue<string>("MongoDb:usersCollection");

            _userCollection = database.GetCollection<User>(usersCollectionName);
        }

        public async Task<User> GetUserById(string userId, CancellationToken cancellationToken = default)
        {
            var cursor = await _userCollection.FindAsync(a => a.Id == userId);

            var user = await cursor.FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        public async Task<User> CreateUser(User user, CancellationToken cancellationToken = default)
        {
            await _userCollection.InsertOneAsync(user, cancellationToken: cancellationToken);

            return user;
        }

        public void UpdateUser(string userId, User user)
        {
            _userCollection.ReplaceOne(a => a.Id == userId, user);
        }

        public void DeleteUserById(string userId)
        {
            _userCollection.DeleteOne(a => a.Id == userId);
        }
        
        public async Task<List<User>> GetUsers(CancellationToken cancellationToken = default)
        {
            var cursor = await _userCollection.FindAsync(a => true);

            var user = await cursor.ToListAsync(cancellationToken);

            return user;
        }
    }
}
