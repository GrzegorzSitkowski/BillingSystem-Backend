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

namespace BillingSystem.Application.Logic.Payments
{
    public static class ListQuery
    {
        public class Request : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Payment> Payments { get; set; } = new List<Payment>();

            public class Payment
            {
                public required int Id { get; set; }
                public int CustomerId { get; set; }
                public double Amount { get; set; }         
                public DateTimeOffset CreateDate { get; set; }
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

                var data = await _applicationDbContext.Payments
                    .OrderByDescending(c => c.CreateDate)
                    .Select(c => new Result.Payment()
                    {
                        Id = c.Id,
                        CustomerId = c.CustomerId,
                        Amount = c.Amount,                     
                        CreateDate = c.CreateDate,
                    })
                    .ToListAsync();

                return new Result()
                {
                    Payments = data
                };
            }
        }
    }
}
