using ExampleElastichSearch.API.Entity.Base;

namespace Member.Api.Models.Base
{
    public class MemberDetailModel : MemberModel
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
