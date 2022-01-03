namespace ExampleElastichSearch.API.Elasticsearch.Model
{
    public class SearchModel
    {
        public SearchModel()
        {
            Skip = 0;
            Take = 10;
            IncludeFields = null;
            PreTags = " <strong style =\"color: red;\">";
            PostTags = "</strong";
        }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool Highlight { get; set; }
        public bool Include { get; set; }
        public string PreTags { get; set; }
        public string PostTags { get; set; }
        public string SearchArea { get; set; }
        public string[] HighlightFields { get; set; }
        public string[] IncludeFields { get; set; }

    }
}
