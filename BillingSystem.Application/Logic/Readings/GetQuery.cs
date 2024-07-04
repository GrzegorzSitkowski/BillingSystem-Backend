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

namespace BillingSystem.Application.Logic.Readings
{
    public static class GetQuery
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        public class Result
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

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Readings.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);

                if(model == null)
                {
                    throw new UnauthorizedException();
                }

                return new Result()
                {
                    Lessons = model.Lessons,
                    Price = model.Price,
                    Period = model.Period,
                    CustomerId = model.CustomerId,
                    CustomerName = model.CustomerName,
                    Invoiced = model.Invoiced,
                    CreatedBy = model.CreatedBy,
                    CreateDate = model.CreateDate
                };
            }
        }
    }
}
