using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Files;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Database.Collections
{
    public class FileCollection : IFileCollection
    {
        private IMongoCollection<File> _fileCollection;

        public FileCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var fileCollectionName = configuration.GetValue<string>("MongoDb:FileCollection");

            _fileCollection = database.GetCollection<File>(fileCollectionName);
        }

        public async Task<File> DownloadImage(string imageId, CancellationToken cancellationToken = default)
        {
            var cursor = await _fileCollection.FindAsync(d => d.FileId == imageId);
            var image = await cursor.FirstOrDefaultAsync(cancellationToken);
            return image;
        }

        public async Task<File> CreateImage(File file, CancellationToken cancellationToken = default)
        {
            await _fileCollection.InsertOneAsync(file);
            return file;
        }

        public void DeleteImageById(string imageId)
        {
            _fileCollection.DeleteOne(d => d.FileId == imageId);
        }
    }
}