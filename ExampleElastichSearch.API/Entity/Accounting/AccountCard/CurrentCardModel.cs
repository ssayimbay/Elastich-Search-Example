using Member.Api.Models.Base;
using System;

namespace Member.Api.Models.Accounting.AccountCard
{
    public class CurrentCardModel
    {
        public Guid Id { get; set; }
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public MemberDetailModel Member { get; set; }
        public string AccountingMemberType { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool Abroad { get; set; }
        public string Iban { get; set; }
    }

}
