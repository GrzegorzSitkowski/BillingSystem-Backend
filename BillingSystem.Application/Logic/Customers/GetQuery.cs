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
    public static class GetQuery
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        public class Result
        {
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string PostCode { get; set; }
            public string City { get; set; }
            public string Email { get; set; }
            public double PayRate { get; set; }
            public double Balance { get; set; }
            public ICollection<Invoice> Invoices { get; set; } 
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);

                if(model == null)
                {
                    throw new UnauthorizedException();
                }

                return new Result()
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    PostCode = model.PostCode,
                    City = model.City,
                    Email = model.Email,
                    PayRate = model.PayRate,
                    Balance = model.Balance,
                    Invoices = model.Invoices.ToList()
                };
            }
        }
    }
}
