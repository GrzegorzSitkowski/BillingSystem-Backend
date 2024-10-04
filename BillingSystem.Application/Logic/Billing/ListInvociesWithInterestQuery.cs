using BillingSystem.Application.Exceptions;
using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Logic.Abstractions;
using BillingSystem.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Logic.Billing
{
    public static class ListInvoiceWithInterest
    {
        public class Request : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Invoice> Invoices { get; set; } = new List<Invoice>();

            public class Invoice
            {
                public required int Id { get; set; }
                public double Amount { get; set; }
                public int CustomerId { get; set; }
                public DateTimeOffset DueDate { get; set; }
                public string Paid { get; set; }
            }           
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var accout = await _currentAccountProvider.GetAuthenticatedAccount();              

                var data = await _applicationDbContext.Invoices.Where(c => c.Paid == "No".ToUpper().ToLower() 
                && (c.DueDate.Day > DateTimeOffset.Now.Day))
                    .OrderByDescending(c => c.CreateDate)
                    .Select(c => new Result.Invoice()
                    {
                        Id = c.Id,
                        Amount = c.Amount,
                        CustomerId = c.CustomerId,
                        DueDate = c.DueDate,
                        Paid = c.Paid,
                    })
                    .ToListAsync();

                return new Result()
                {
                    Invoices = data
                };
            }
        }
    }
}
