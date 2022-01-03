using System;

namespace Member.Api.Models.Accounting.Invoice
{
    public class InvoiceDetailModel
    {
        public Guid Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VatRate { get; set; }
        public string Description { get; set; }
    }
}
