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
    public static class CreateOrUpdateCommand
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
            public double Lessons { get; set; }
            public int Price { get; set; }
            public string Period { get; set; }
            public int CustomerId { get; set; }
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

                Domain.Entities.Reading? model = null;
                if (request.Id.HasValue)
                {
                    model = await _applicationDbContext.Readings.FirstOrDefaultAsync(u => u.Id == request.Id && u.CreatedBy == account.Id);
                }
                else
                {
                    var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == request.CustomerId);
                    model = new Domain.Entities.Reading()
                    {
                        CreatedBy = account.Id,
                        CustomerName = customer.FullName,
                        Invoiced = 0
                    };

                    _applicationDbContext.Readings.Add(model);
                }

                if (model == null)
                {
                    throw new UnauthorizedException();
                }

                model.Lessons = request.Lessons;
                model.Price = request.Price;
                model.Period = request.Period;
                model.CustomerId = request.CustomerId;

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
                RuleFor(x => x.Lessons).NotEmpty();
                RuleFor(x => x.Price).NotEmpty();
                RuleFor(x => x.Period).NotEmpty().MaximumLength(30);
                RuleFor(x => x.CustomerId).NotEmpty();
            }
        }
    }
}
