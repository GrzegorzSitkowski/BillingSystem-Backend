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
    public static class GetQuery
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public int CustomerId { get; set; }
            public int DocumentId { get; set; }
            public string DocumentNumber { get; set; }
            public double Amount { get; set; }
            public DateTimeOffset CreateDate { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Payments.FirstOrDefaultAsync(c => c.Id == request.Id);

                if(model == null)
                {
                    throw new UnauthorizedException();
                }

                return new Result()
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    DocumentId = model.DocumentId,
                    DocumentNumber = model.DocumentNumber,
                    Amount = model.Amount,                   
                    CreateDate = model.CreateDate,
                };
            }
        }
    }
}
