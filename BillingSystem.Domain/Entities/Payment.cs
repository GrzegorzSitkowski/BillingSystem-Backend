using BillingSystem.Domain.Common;

namespace BillingSystem.Domain.Entities
{
    public class Payment : DomainEntity
    {
        public int CustomerId { get; set; }   
        public string DocumentNumber { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public Customer Customer { get; set; } = default!;
        public int DocumentId { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = default!;
    }
}
