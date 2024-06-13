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

namespace BillingSystem.Application.Logic.Customer
{
    public static class CreateOrUpdateCustomerCommand
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
            public required string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string PostCode { get; set; }
            public string City { get; set; }
            public string Email { get; set; }
            public double PayRate { get; set; }
            public double Balance { get; set; }
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

                Domain.Entities.Customer? model = null;
                if (request.Id.HasValue)
                {
                    model = await _applicationDbContext.Customers.FirstOrDefaultAsync(u => u.Id == request.Id && u.CreatedBy == account.Id);
                }
                else
                {
                    model = new Domain.Entities.Customer()
                    {
                        CreatedBy = account.Id
                    };

                    _applicationDbContext.Customers.Add(model);
                }

                if (model == null)
                {
                    throw new UnauthorizedException();
                }

                model.FullName = request.FullName;
                model.PhoneNumber = request.PhoneNumber;
                model.Address = request.Address;
                model.PostCode = request.PostCode;
                model.City = request.City;
                model.Email = request.Email;
                model.PayRate = request.PayRate;
                model.Balance = request.Balance;

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
                RuleFor(x => x.FullName).NotEmpty();
                RuleFor(x => x.FullName).MaximumLength(100);
                RuleFor(x => x.PhoneNumber).MaximumLength(50);
                RuleFor(x => x.Address).MaximumLength(100);
                RuleFor(x => x.PostCode).MaximumLength(50);
                RuleFor(x => x.City).MaximumLength(100);
                RuleFor(x => x.Email).MaximumLength(100);
                RuleFor(x => x.PayRate).NotEmpty();
                RuleFor(x => x.Balance).NotEmpty();
            }
        }
    }
}
