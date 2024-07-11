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
        public float Amount { get; set; }
        public int CsutomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string StatusInvoice { get; set; }
        public string StatusPayment { get; set; }
        public Customer Customer { get; set; } = default!;
    }
}
