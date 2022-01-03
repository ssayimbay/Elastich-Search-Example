using ExampleElastichSearch.API.Entity.Base;
using System;
using System.Collections.Generic;

namespace ExampleElastichSearch.API.Entity
{
    public class StudentListModel
    {
        public StudentListModel()
        {
            Terms = new List<StudentTermListModel>();
            Employees = new List<KeyValuePair<string, string>>();
        }

        public Guid MemberId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? Gender { get; set; }
        public BrandModel Brand { get; set; }
        public List<StudentTermListModel> Terms { get; set; }
        public List<KeyValuePair<string, string>> Employees { get; set; }
    }
}