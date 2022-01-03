using System;

namespace ExampleElastichSearch.API.Entity.Base
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }
        public string Department { get; set; }
        public string FullName { get; set; }
        public int Type { get; set; }
    }
}