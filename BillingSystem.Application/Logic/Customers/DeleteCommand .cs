﻿using BillingSystem.Application.Exceptions;
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

namespace BillingSystem.Application.Logic.Customers
{
    public static class DeleteCommand
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        public class Result
        {
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);

                if(model == null)
                {
                    throw new UnauthorizedException();
                }

                _applicationDbContext.Customers.Remove(model);

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return new Result();
            }
        }
    }
}
