using BillingSystem.Application.Exceptions;
using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Logic.Abstractions;
using BillingSystem.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Application.Logic.Payments
{
    public static class CreateCommand
    {
        public class Request : IRequest<Result>
        {
            public int CustomerId { get; set; }
            public int DocumentId { get; set; }
            public double Amount { get; set; }
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
                var invoice = await _applicationDbContext.Invoices.FirstOrDefaultAsync(i => i.Id == request.DocumentId);

                var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == request.CustomerId);
                    
                var model = new Domain.Entities.Payment()
                    {
                        CustomerId = customer.Id,
                        DocumentId = invoice.Id,
                        DocumentNumber = invoice.DocumentNumber,
                        Amount = request.Amount,
                    };

                customer.Balance += model.Amount;
                if(model.Amount >= invoice.Amount)
                {
                    invoice.Paid = "Yes";
                }

                _applicationDbContext.Payments.Add(model);

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
                RuleFor(x => x.DocumentId).NotEmpty();
                RuleFor(x => x.CustomerId).NotEmpty();
                RuleFor(x => x.Amount).NotEmpty();
            }
        }
    }
}
