using BillingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<AccountUser> AccountUsers { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Reading> Readings { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
