using BillingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.Entities
{
    public class InterestNote : DomainEntity
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public double Amount { get; set; }
        public string DocumentNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
