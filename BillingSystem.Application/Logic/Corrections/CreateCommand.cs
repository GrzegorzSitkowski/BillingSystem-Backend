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
                var invoice = await _applicationDbContext.Invoices.FirstOrDefaultAsync(i => i.Id == request.ReadingId);

                var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == reading.CustomerId);
                    
                var model = new Invoice()
                    {
                        ReadingId = reading.Id,
                        CreatedBy = account.Id,
                        CustomerId = customer.Id,
                        CustomerName = customer.FullName,
                        Amount = (reading.Lessons * reading.Price) * customer.PayRate
                    };

                customer.Balance -= model.Amount;

                _applicationDbContext.Invoices.Add(model);

                if (model == null)
                {
                    throw new UnauthorizedException();
                }

                model.ReadingId = request.ReadingId;

                reading.Invoiced = 1;

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
                RuleFor(x => x.ReadingId).NotEmpty();
            }
        }
    }
}
