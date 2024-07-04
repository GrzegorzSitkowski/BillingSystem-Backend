using BillingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Domain.Entities
{
    public class Reading : DomainEntity
    {
        public double Lessons { get; set; }
        public int Price { get; set; }
        public string Period { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Invoiced { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
    }
}
