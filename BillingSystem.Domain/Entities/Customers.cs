using BillingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.Entities
{
    public class Customers : DomainEntity
    {
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public float PayRate { get; set; }
        public float Balance { get; set; }
        //public List<Invoice> Invoices { get; set; }
    }
}
