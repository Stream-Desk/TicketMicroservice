using Domain.Users;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Colections
{
    public class UsersCollection : IUsersCollection
    {
        private IMongoCollection<Users> _usersCollection;

        public UsersCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var usersCollectionName = configuration.GetValue<string>("MongoDb:usersCollection");

            _usersCollection = database.GetCollection<Userss>(usersCollectionName);
        }

        public async Task<Users> CreateUsers(Userss tickets, CancellationToken cancellationToken = default)
        {
            await _usersCollection.InsertOneAsync(userss);

            return userss;
        }

        public void DeleteUsersById(string usersId)
        {
            _usersCollection.DeleteOne(a => a.Id == usersId);
        }

        public async Task<Users> GetusersById(string usersId, CancellationToken cancellationToken = default)
        {
            var cursor = await _usersCollection.FindAsync(a => a.Id == usersId);

            var users = await cursor.FirstOrDefaultAsync(cancellationToken);

            return userss;
        }

        public async Task<List<Users>> GetUsers(CancellationToken cancellationToken = default)
        {
            var cursor = await _usersCollection.FindAsync(a => true);

            var tickets = await cursor.ToListAsync(cancellationToken);

            return tickets;
        }

        public void UpdateAuthor(string usersId, Users users)
        {
            _userssCollection.ReplaceOne(a => a.Id == usersId, users);
        }
    }
}
