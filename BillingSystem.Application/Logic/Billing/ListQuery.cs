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

namespace BillingSystem.Application.Logic.Billing
{
    public static class ListQuery
    {
        public class Request : IRequest<Result>
        {
        }

        public class Result
        {
            public List<Reading> Readings { get; set; } = new List<Reading>();

            public class Reading
            {
                public required int Id { get; set; }
                public double Lessons { get; set; }
                public int Price { get; set; }
                public string Period { get; set; }
                public int CustomerId { get; set; }
                public string CustomerName { get; set; }
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

                var data = await _applicationDbContext.Readings.Where(c => c.Invoiced == 0)
                    .OrderByDescending(c => c.CreateDate)
                    .Select(c => new Result.Reading()
                    {
                        Id = c.Id,
                        Lessons = c.Lessons,
                        Price = c.Price,
                        Period = c.Period,
                        CustomerName = c.CustomerName,
                        CustomerId = c.CustomerId
                    })
                    .ToListAsync();

                return new Result()
                {
                    Readings = data
                };
            }
        }
    }
}
