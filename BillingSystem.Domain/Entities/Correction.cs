using BillingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.Entities
{
    public class Correction : DomainEntity
    {
        public double Amount { get; set; }
        public string DocumentNumber { get; set; }
        public string Reason { get; set; }
        public string Describe { get; set; }
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; }
    }
}
