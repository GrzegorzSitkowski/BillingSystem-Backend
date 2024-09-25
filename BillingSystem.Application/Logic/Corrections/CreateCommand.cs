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

namespace BillingSystem.Application.Logic.Corrections
{
    public static class CreateCommand
    {
        public class Request : IRequest<Result>
        {
            public double Amount { get; set; }
            public string Reason { get; set; }
            public string Describe { get; set; }
            public int InvoiceId { get; set; }
        }

        public class Result
        {
            public required int Id { get; set; }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();
                var invoice = await _applicationDbContext.Invoices.FirstOrDefaultAsync(i => i.Id == request.InvoiceId);

                var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == invoice.CustomerId);
                    
                var model = new Correction()
                    {
                        Amount = request.Amount,
                        DocumentNumber = $"{invoice.Id}/{customer.Id}/{invoice.CreateDate.Month}/{invoice.CreateDate.Year}",
                        Reason = request.Reason,
                        Describe = request.Describe,
                        InvoiceId = invoice.Id,
                        CustomerId = customer.Id,
                        CreatedBy = account.Id
                    };

                customer.Balance += model.Amount;

                _applicationDbContext.Corrections.Add(model);

                if (model == null)
                {
                    throw new UnauthorizedException();
                }

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return new Result()
                {
                    Id = model.Id
                };
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Amount).NotEmpty();
                RuleFor(x => x.InvoiceId).NotEmpty();
                RuleFor(x => x.Reason).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Describe).NotEmpty().MaximumLength(100);
            }
        }
    }
}
