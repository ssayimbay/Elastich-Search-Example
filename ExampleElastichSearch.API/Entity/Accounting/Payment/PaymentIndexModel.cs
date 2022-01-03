using System;
using ExampleElastichSearch.API.Entity.Base;
using Member.Api.Models.Base;

namespace Member.Api.Models.Accounting.Payment
{
    public class PaymentIndexModel
    {
        public int Id { get; set; }
        public BrandModel Brand { get; set; }
        public string PaymentSource { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public string StatusMessage { get; set; }
        public MemberDetailModel Member { get; set; }
        public string PackageName { get; set; }
        public string PackageDetail { get; set; }
        public string TotalAmount { get; set; }
        public string BankName { get; set; }
        public string Installment { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}