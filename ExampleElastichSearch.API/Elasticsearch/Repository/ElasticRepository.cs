using Elasticsearch.Net;
using ExampleElastichSearch.API.Elasticsearch.Data;
using ExampleElastichSearch.API.Elasticsearch.Entity;
using ExampleElastichSearch.API.Elasticsearch.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleElastichSearch.API.Elasticsearch.Repository.Elasticsearch
{
    public class Repository<T, TId> : IRepository<T, TId> where T : ElasticEntity<TId>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticsearchSettings _elasticsearchSettings;

        public Repository(IOptions<ElasticsearchSettings> elasticsearchSettings)
        {

            _elasticsearchSettings = elasticsearchSettings.Value;
            _elasticClient = GetClient();
        }

        private string _aliasName => typeof(T).Name.ToLower();
        private string _indexName => $"{typeof(T).Name}-index".ToLower();
        private ElasticClient GetClient()
        {
            var node = new Uri($"{_elasticsearchSettings.Host}:{_elasticsearchSettings.Port}");
            var connectionString = new ConnectionSettings(node)
                .DisablePing()
                .SniffOnStartup(false)
                .SniffOnConnectionFault(false);

            connectionString.BasicAuthentication(_elasticsearchSettings.Username, _elasticsearchSettings.Password);

            return new ElasticClient(connectionString);
        }

        public async Task CreateIndexAsync()
        {
            var existResponse = await _elasticClient.Indices.ExistsAsync(_aliasName);
            if (existResponse.Exists) throw new Exception($"{_aliasName} already exists.");

            var createIndexResponse = await _elasticClient.Indices.CreateAsync(_indexName,
                ss => ss.Index(_indexName)

                        .Settings(o => o.NumberOfShards(4)
                                       .NumberOfReplicas(2)
                                       .Analysis(a => a.TokenFilters(tf => tf.AsciiFolding("my_ascii_folding", af => af.PreserveOriginal(true)))
                                                       .Analyzers(aa => aa.Custom("turkish_analyzer", ca => ca.Filters("lowercase", "my_ascii_folding").Tokenizer("standard")))))
                        .Map<T>(m => m.AutoMap(typeof(T))
                                      .Properties<T>(p => p.Text(t => t.Name(n => n.SearchArea).Analyzer("turkish_analyzer")))));

            if (createIndexResponse.Acknowledged)
            {
                await _elasticClient.Indices.BulkAliasAsync(al => al.Add(add => add.Index(_indexName).Alias(_aliasName)));
                return;
            }

            throw new Exception($"Create Index {_aliasName} failed : {createIndexResponse.ServerError.Error.Reason}");
        }

        public async Task DeleteIndexAsync()
        {

            var deleteIndexResponse = await _elasticClient.Indices.DeleteAsync(_indexName);
            if (deleteIndexResponse.Acknowledged) return;
            throw new Exception($"Delete index {_aliasName} failed :{deleteIndexResponse.ServerError.Error.Reason}");
        }

        public async Task AddOrUpdate(T entity)
        {
            var existResponse = _elasticClient.DocumentExists<T>(entity, dd => dd.Index(_aliasName));

            if (existResponse.Exists)
            {
                var updateResponse = await _elasticClient.UpdateAsync<T>(entity,
                    ss => ss.Index(_aliasName).Doc(entity).RetryOnConflict(3));

                if (updateResponse.ServerError == null) return;
                throw new Exception($"Update document failed at index({_aliasName}) : {updateResponse.ServerError.Error.Reason}");
            }

            var indexResponse = await _elasticClient.IndexAsync(entity, ss => ss.Index(_aliasName));

            if (indexResponse.ServerError == null) return;
            throw new Exception($"Insert document failed at index({_aliasName}) : {indexResponse.ServerError.Error.Reason}");
        }

        public async Task DeleteAsync(T entity)
        {
            var deleteResponse = await _elasticClient.DeleteAsync<T>(entity, ss => ss.Index(_aliasName));
            if (deleteResponse.ServerError == null) return;
            throw new Exception($"Delete docuemnt at index({_aliasName}) :{deleteResponse.ServerError.Error.Reason}");
        }

        public async Task BulkAddOrUpdateAsync(IEnumerable<T> entities)
        {
            var bulkResponse = await _elasticClient.BulkAsync(b => b.IndexMany(entities, (d, doc) => d.Document(doc).Index(_aliasName)));
            if (bulkResponse.ServerError == null) return;
            throw new Exception($"Bulk AddtOrUpdate docuemnt failed at index({_aliasName}) :{bulkResponse.ServerError.Error.Reason}");
        }

        public async Task BulkDeleteAsync(IEnumerable<T> entities)
        {
            var bulkResponse = await _elasticClient.DeleteManyAsync(entities, _aliasName);
            if (bulkResponse.ServerError == null) return;
            throw new Exception($"Bulk delete docuemnt failed at index({_aliasName}) :{bulkResponse.ServerError.Error.Reason}");
        }

        public async Task<IEnumerable<T>> SearchAsync(SearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException($"{nameof(searchModel)} is null");
            var query = new SearchDescriptor<T>();
            query = query.Index(_aliasName);
            query = query.Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.SearchArea).Query(searchModel.SearchArea)));
            query = AddHightlightIfEnable(query, searchModel);
            query = AddIncludeFieldsIfEnable(query, searchModel);
            query = query.Skip(searchModel.Skip).Take(searchModel.Take);

            var searchResponse = await _elasticClient.SearchAsync<T>(query);

            if (searchResponse.ServerError != null)
                throw new Exception($"Search failed for index({_aliasName}) :{searchResponse.ServerError.Error.Reason}");

            return searchResponse.Documents;
        }

        public async Task<IEnumerable<T>> SuggerSearchAsync(string suggerText)
        {
            var query = new SearchDescriptor<T>();
            query.Index(_aliasName)
                 .Suggest(su => su.Completion($"{_aliasName}_suggestions",
                                         c => c.Field(f => f.Suggest)
                                               .Analyzer("simple")
                                               .Prefix(suggerText)
                                               .Fuzzy(f => f.Fuzziness(Fuzziness.Auto))));

            var searchResponse = await _elasticClient.SearchAsync<T>(query);

            if (searchResponse.ServerError != null)
                throw new Exception($"Search failed for index({_aliasName}) :{searchResponse.ServerError.Error.Reason}");

            return searchResponse.Documents;
        }

        private SearchDescriptor<T> AddHightlightIfEnable(SearchDescriptor<T> query, SearchModel searchModel)
        {
            if (!searchModel.Highlight) return query;
            if (searchModel.HighlightFields == null && searchModel.HighlightFields.Length == 0) throw new Exception($"Highlight enable but HightlightFields is null.");

            var highlightDescriptor = new HighlightDescriptor<T>();
            highlightDescriptor.PreTags(searchModel.PreTags).PostTags(searchModel.PostTags);
            var highlightFieldDescriptor = new List<Func<HighlightFieldDescriptor<T>, IHighlightField>>();

            foreach (var field in searchModel.HighlightFields)
            {
                highlightFieldDescriptor.Add(f => f.Field(field));
            }

            highlightDescriptor.Fields(highlightFieldDescriptor.ToArray());

            return query.Highlight(h => highlightDescriptor);
        }

        private SearchDescriptor<T> AddIncludeFieldsIfEnable(SearchDescriptor<T> query, SearchModel searchModel)
        {
            if (!searchModel.Include) return query;
            if (searchModel.IncludeFields == null && searchModel.IncludeFields.Length == 0) throw new Exception($"Include enable but IncludeFields is null.");

            return query.Source(ss => ss.Includes(ff => ff.Fields(searchModel.IncludeFields)));
        }
    }
}

