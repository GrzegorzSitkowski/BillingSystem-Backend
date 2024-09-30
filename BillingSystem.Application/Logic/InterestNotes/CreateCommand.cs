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
    public static class CreateCommand
    {
        public class Request : IRequest<Result>
        {
            //public int? Id { get; set; }
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
                    
                var model = new InterestNote()
                    {
                        InvoiceId = invoice.Id,
                        CustomerId = customer.Id,
                        Amount = (DateTimeOffset.Now - invoice.DueDate).Days * (invoice.Amount * 0.002),
                        DocumentNumber = $"NC/{invoice.Id}/{customer.Id}/{invoice.CreateDate.Month}/{invoice.CreateDate.Year}",
                        InvoiceNumber = invoice.DocumentNumber,
                        CreatedBy = account.Id,
                    };

                customer.Balance -= model.Amount;

                _applicationDbContext.InterestNotes.Add(model);

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
                RuleFor(x => x.InvoiceId).NotEmpty();
            }
        }
    }
}
