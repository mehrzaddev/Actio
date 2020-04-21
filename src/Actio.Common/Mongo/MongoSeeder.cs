using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Actio.Common.Mongo
{
    public class MongoSeeder : IDatabaseSeeder
    {
        protected readonly IMongoDatabase Database;

        public MongoSeeder(IMongoDatabase database)
        {
            Database = database;
        }
        public async Task SeedAsynd()
        {
            var collectikonsCursor = await Database.ListCollectionsAsync();
            var collections = await collectikonsCursor.ToListAsync();

            if (collections.Any())
            {
                return;
            }

            await CustomSeedAsync();
        }

        protected virtual async Task CustomSeedAsync()
        {
            await Task.CompletedTask;
        }
    }
}
