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

namespace BillingSystem.Application.Logic.InterestNotes
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
            public int InvoiceId { get; set; }
            public int CustomerId { get; set; }
            public double Amount { get; set; }
            public string DocumentNumber { get; set; }
            public string InvoiceNumber { get; set; }
            public DateTimeOffset DueDate { get; set; }
            public DateTimeOffset CreateDate { get; set; }
            public int CreatedBy { get; set; }      
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.InterestNotes.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);

                if(model == null)
                {
                    throw new UnauthorizedException();
                }

                return new Result()
                {
                    Id = model.Id,
                    InvoiceId = model.InvoiceId,
                    CustomerId = model.CustomerId,
                    Amount = model.Amount,
                    DocumentNumber = model.DocumentNumber,
                    InvoiceNumber = model.InvoiceNumber,
                    DueDate = model.DueDate,
                    CreateDate = model.CreateDate,
                    CreatedBy = model.CreatedBy          
                };
            }
        }
    }
}
