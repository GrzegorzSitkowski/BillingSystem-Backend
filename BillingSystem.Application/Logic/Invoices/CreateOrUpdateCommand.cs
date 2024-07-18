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

namespace BillingSystem.Application.Logic.Invoices
{
    public static class CreateOrUpdateCommand
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
            public int ReadingId { get; set; }
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
                var reading = await _applicationDbContext.Readings.FirstOrDefaultAsync(i => i.Id == request.ReadingId);

                Invoice? model = null;
                if (request.Id.HasValue)
                {
                    model = await _applicationDbContext.Invoices.FirstOrDefaultAsync(u => u.Id == request.Id && u.CreatedBy == account.Id);
                }
                else
                {
                    
                    var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == reading.CustomerId);
                    
                    model = new Invoice()
                    {
                        CreatedBy = account.Id,
                        CustomerId = customer.Id,
                        CustomerName = customer.FullName,
                        StatusInvoice = "Test",
                        StatusPayment = "Not paid",
                        Amount = (reading.Lessons * reading.Price) * customer.PayRate
                    };

                    customer.Balance -= model.Amount;

                    _applicationDbContext.Invoices.Add(model);
                }

                if (model == null)
                {
                    throw new UnauthorizedException();
                }

                model.ReadingId = request.ReadingId;

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
