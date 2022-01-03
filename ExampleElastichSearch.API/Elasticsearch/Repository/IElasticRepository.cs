using ExampleElastichSearch.API.Elasticsearch.Entity;
using ExampleElastichSearch.API.Elasticsearch.Model;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleElastichSearch.API.Elasticsearch.Repository.Elasticsearch
{
    public interface IRepository<T,TId> where T : ElasticEntity<TId>
    {
        Task CreateIndexAsync();
        Task DeleteIndexAsync();
        Task AddOrUpdate(T entity);
        Task DeleteAsync(T entity);
        Task BulkAddOrUpdateAsync(IEnumerable<T> entities);
        Task BulkDeleteAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> SearchAsync(SearchModel searchModel);
        Task<IEnumerable<T>> SuggerSearchAsync(string suggerText);
    }
}
