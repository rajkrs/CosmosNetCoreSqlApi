using CosmosNetCoreSqlApi.Enums;
using CosmosNetCoreSqlApi.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosNetCoreSqlApi.Services
{
    public class ContactService : BaseDbService<Contact>, IContactService
    {
        public ContactService(IServiceProvider serviceProvider) : base(serviceProvider, DbNames.Identity.ToString())
        {

        }
        public async Task AddAsync(Contact Contact)
        {
            await DbContainer.CreateItemAsync<Contact>(Contact, new PartitionKey(Contact.Id.ToString()));
        }

        public async Task DeleteAsync(string id)
        {
            await DbContainer.DeleteItemAsync<Contact>(id, new PartitionKey(id));
        }

        public async Task<Contact> GetAsync(string id)
        {
            try
            {
                ItemResponse<Contact> response = await DbContainer.ReadItemAsync<Contact>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateAsync(string id, Contact Contact)
        {
            await DbContainer.UpsertItemAsync<Contact>(Contact, new PartitionKey(id));
        }
    }
}
