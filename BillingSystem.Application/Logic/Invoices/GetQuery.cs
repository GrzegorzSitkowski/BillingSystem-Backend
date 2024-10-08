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

namespace BillingSystem.Application.Logic.Invoices
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
            public double Amount { get; set; }
            public string DocumentNumber { get; set; }
            public int ReadingId { get; set; }
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public DateTimeOffset CreateDate { get; set; }
            public int CreatedBy { get; set; }
            public DateTimeOffset DueDate { get; set; }
            public string Paid { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Invoices.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);

                if(model == null)
                {
                    throw new UnauthorizedException();
                }

                return new Result()
                {
                    Id = model.Id,
                    Amount = model.Amount,
                    DocumentNumber = model.DocumentNumber,
                    ReadingId = model.ReadingId,
                    CustomerId = model.CustomerId,
                    CustomerName = model.CustomerName,
                    CreateDate = model.CreateDate,
                    CreatedBy = model.CreatedBy,
                    DueDate = model.DueDate,
                    Paid = model.Paid,
                };
            }
        }
    }
}
