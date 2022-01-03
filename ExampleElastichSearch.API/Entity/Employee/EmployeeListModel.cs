using System;
using ExampleElastichSearch.API.Elasticsearch.Entity;
using ExampleElastichSearch.API.Entity.Base;

namespace ExampleElastichSearch.API.Entity.Employee
{
    public class EmployeeListModel:ElasticEntity<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string CorporateEmail { get; set; }
        public string CorporatePhone { get; set; }
        public BrandModel Brand { get; set; }
        public EmployeeTypeModel EmployeeType { get; set; }
    }
}