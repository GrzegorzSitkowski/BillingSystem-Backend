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

namespace BillingSystem.Application.Logic.Customers
{
    public static class ListQuery
    {
        public class Request : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Customer> Customers { get; set; } = new List<Customer>();

            public class Customer
            {
                public required int Id { get; set; }
                public string FullName { get; set; }
                public string City { get; set; }
                public string Email { get; set; }
                public double PayRate { get; set; }
                public double Balance { get; set; }
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

                var data = await _applicationDbContext.Customers.Where(c => c.CreatedBy == accout.Id)
                    .OrderByDescending(c => c.CreateDate)
                    .Select(c => new Result.Customer()
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        City = c.City,
                        Email = c.Email,
                        PayRate = c.PayRate,
                        Balance = c.Balance
                    })
                    .ToListAsync();

                return new Result()
                {
                    Customers = data
                };
            }
        }
    }
}
