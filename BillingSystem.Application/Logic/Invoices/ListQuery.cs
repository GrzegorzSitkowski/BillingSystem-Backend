using BillingSystem.Application.Exceptions;
using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Logic.Abstractions;
using BillingSystem.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Logic.Invoices
{
    public static class ListQuery
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
                public int CustomerId { get; set; }
                public DateTimeOffset CreateDate { get; set; }
                public DateTimeOffset DueDate { get; set; }
                public string StatusInvoice { get; set; }
                public string StatusPayment { get; set; }
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

                var data = await _applicationDbContext.Invoices.Where(c => c.CreatedBy == accout.Id)
                    .OrderByDescending(c => c.CreateDate)
                    .Select(c => new Result.Invoice()
                    {
                        Id = c.Id,
                        CustomerId = c.CustomerId,
                        CreateDate = c.CreateDate,
                        DueDate = c.DueDate,
                        StatusInvoice = c.StatusInvoice,
                        StatusPayment = c.StatusPayment
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
