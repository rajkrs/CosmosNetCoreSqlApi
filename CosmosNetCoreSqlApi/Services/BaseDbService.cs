using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CosmosNetCoreSqlApi.Services
{
    public class BaseDbService<T>
    {
        public Container DbContainer;
        public  BaseDbService(IServiceProvider serviceProvider, string dbName)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var configurationSection = configuration.GetSection("CosmosDb");
            
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;

            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            CosmosClient dbClient = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();

            DatabaseResponse database = dbClient.CreateDatabaseIfNotExistsAsync(dbName).GetAwaiter().GetResult();

            var partitionKey = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(false).Where(a => a.GetType() == typeof(KeyAttribute)).Count() > 0).FirstOrDefault();
            database.Database.CreateContainerIfNotExistsAsync(typeof(T).Name, $"/{partitionKey.Name.ToLower()}").GetAwaiter().GetResult();
 
            DbContainer = dbClient.GetContainer(dbName, typeof(T).Name);
        }

        public async Task<IEnumerable<T>> GetQueryAsync(string queryString)
        {
            var query = DbContainer.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }
            return results;
        }
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression)
        {
            var setIterator = DbContainer.GetItemLinqQueryable<T>().Where(expression).ToFeedIterator();
            List<T> results = new List<T>();
            while (setIterator.HasMoreResults)
            {
                var response = await setIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }


    }
}
