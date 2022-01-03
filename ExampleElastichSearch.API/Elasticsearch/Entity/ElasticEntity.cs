using Nest;

namespace ExampleElastichSearch.API.Elasticsearch.Entity
{
    public class ElasticEntity<TId>
    {
        public TId Id { get; set; }
        public CompletionField Suggest { get; set; }
        public string SearchArea { get; set; }
        public double? Score { get; set; }
    }
      
}
