using ExampleElastichSearch.API.Elasticsearch.Entity;
using ExampleElastichSearch.API.Entity.Base;
using Member.Api.Models.Base;
using System;

namespace ExampleElastichSearch.API.Entity.Task
{
    public class TaskIndexModel : ElasticEntity<string>
    {
        public string TaskMessage { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskType { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public MemberDetailModel Member { get; set; }
        public BrandModel Brand { get; set; }
        public int PostponeCount { get; set; }
        public TaskUser User { get; set; }
    }

    public class TaskUser
    {
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
