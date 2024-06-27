using BillingSystem.Application.Exceptions;
using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Logic.Abstractions;
using BillingSystem.Domain.Entities;
using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Logic.Account
{
    public static class CurrentAccountQuery
    {
        public class Request : IRequest<Result>
        {
        }

        public class Result
        {
            public required string Name { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            private readonly IAuthenticationDataProvider _authenticationDataProvider;

            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {
               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                return new Result()
                {
                    Name = account.Name,
                };
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator() 
            {
            }
        }
    }
}
