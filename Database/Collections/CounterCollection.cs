using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Counters;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace Database.Collections
{
    public class CounterCollection : ICounterCollection
    {
        private IMongoCollection<Counter> _counterCollection;

        public CounterCollection()
        {
        }

        public CounterCollection(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            var dbName = configuration.GetValue<string>("MongoDb:Database");
            var database = client.GetDatabase(dbName);
            var counterCollectionName = configuration.GetValue<string>("MongoDb:CounterCollection");

            _counterCollection = database.GetCollection<Counter>(counterCollectionName);




        }
    }

    internal class Counter
    {
    }
}


    
