using BillingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.Entities
{
    public class Invoice : DomainEntity
    {
        public double Amount { get; set; }
        public string DocumentNumber { get; set; }
        public int ReadingId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
        public DateTimeOffset DueDate { get; set; } = DateTimeOffset.Now.AddDays(14);
        public Customer Customer { get; set; } = default!;
    }
}
