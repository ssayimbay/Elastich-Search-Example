using ExampleElastichSearch.API.Entity.Base;
using System;
using System.Collections.Generic;

namespace ExampleElastichSearch.API.Entity
{
    public class CandidateListModel
    {
        public BrandModel Brand { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Level { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApplicationStatus { get; set; }
        public List<EmployeeModel> Employees { get; set; } = new();

        /// <summary>
        /// Like application ios, reference, mailing and etc.
        /// </summary>
        public List<TagModel> MemberTags { get; set; } = new();
    }
}