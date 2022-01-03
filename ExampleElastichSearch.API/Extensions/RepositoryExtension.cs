using ExampleElastichSearch.API.Elasticsearch.Data;
using ExampleElastichSearch.API.Elasticsearch.Repository.Elasticsearch;
using ExampleElastichSearch.API.Entity;
using ExampleElastichSearch.API.Entity.Employee;
using ExampleElastichSearch.API.Entity.Task;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleElastichSearch.API.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ElasticsearchSettings>(configuration.GetSection(nameof(ElasticsearchSettings)));
 
            services.AddScoped<IRepository<EmployeeListModel,string>, Repository<EmployeeListModel, string>>();
            services.AddScoped<IRepository<TaskIndexModel, string>, Repository<TaskIndexModel, string>>();
        }
    }
}
