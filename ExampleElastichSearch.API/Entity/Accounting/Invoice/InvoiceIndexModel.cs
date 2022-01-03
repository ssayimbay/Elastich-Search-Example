using System;
using System.Collections.Generic;
using System.Linq;

namespace Member.Api.Models.Accounting.Invoice
{
    public class InvoiceIndexModel
    {
        public InvoiceIndexModel()
        {
            Details = new List<InvoiceDetailModel>();
        }

        public Guid Id { get; set; }
        public string ProviderId { get; set; }
        public string BankName { get; set; }
        public int PaymentType { get; set; }
        public int Installment { get; set; }
        public DateTime CreateDate { get; set; }
        public string InvoiceNo { get; set; }
        public string Status { get; set; }
        public Guid MemberId { get; set; }
        public string MemberName { get; set; }
        public string AccountingMemberType { get; set; }
        public string Address { get; set; }
        public string TotalAmount => _totalAmount.ToString();
        public string ValueAddedTax => _valueAddedTax.ToString();
        public string NetAmount => _netAmount.ToString();
        public List<InvoiceDetailModel> Details { get; set; }
        private decimal _totalAmount => _valueAddedTax + _netAmount;
        private decimal _valueAddedTax => Details.Sum(x => x.VatRate * x.Quantity * x.UnitPrice) / 100;
        private decimal _netAmount => Details.Sum(x => x.Quantity * x.UnitPrice);
    }
}