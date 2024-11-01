using BillingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.Entities
{
    public class Payment : DomainEntity
    {
        public int CustomerId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentNumber { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public Customer Customer { get; set; } = default!;
        public Invoice Invoice { get; set; } = default!;
    }
}
