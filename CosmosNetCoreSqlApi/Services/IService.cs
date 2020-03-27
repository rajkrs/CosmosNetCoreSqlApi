using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosNetCoreSqlApi.Services
{
    public interface IService<T> 
    {
        Task<IEnumerable<T>> GetQueryAsync(string query);
        Task<T> GetAsync(string id);
        Task AddAsync(T item);
        Task UpdateAsync(string id, T item);
        Task DeleteAsync(string id);
    }
}
