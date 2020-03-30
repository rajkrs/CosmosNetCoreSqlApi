using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosNetCoreSqlApi.Models;
using User = CosmosNetCoreSqlApi.Models.User;
using CosmosNetCoreSqlApi.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosNetCoreSqlApi.Services
{
    public class UserService: BaseDbService<User>, IUserService
    {
        public UserService(IServiceProvider serviceProvider): base(serviceProvider, DbNames.Identity.ToString())
        {
            
        }

        public async Task AddAsync(User user)
        {
            await DbContainer.CreateItemAsync<User>(user, new PartitionKey(user.Id.ToString()));
        }

        public async Task DeleteAsync(string id)
        {
            await DbContainer.DeleteItemAsync<User>(id, new PartitionKey(id));
        }

        public async Task<User> GetByIdAsync(string id)
        {
            try
            {
                ItemResponse<User> response =await DbContainer.ReadItemAsync<User>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateAsync(string id, User user)
        {
            await DbContainer.UpsertItemAsync<User>(user, new PartitionKey(id));
        }
    }
}
