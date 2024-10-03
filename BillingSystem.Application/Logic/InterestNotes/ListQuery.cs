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
    public static class ListQuery
    {
        public class Request : IRequest<Result>
        {
        }

        public class Result
        {
            public List<InterestNote> InterestNotes { get; set; } = new List<InterestNote>();

            public class InterestNote
            {
                public required int Id { get; set; }
                public int CustomerId { get; set; }
                public double Amount { get; set; }
                public string DocumentNumber { get; set; }              
                public DateTimeOffset CreateDate { get; set; }
            }           
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {               
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var accout = await _currentAccountProvider.GetAuthenticatedAccount();

                var data = await _applicationDbContext.InterestNotes.Where(c => c.CreatedBy == accout.Id)
                    .OrderByDescending(c => c.CreateDate)
                    .Select(c => new Result.InterestNote()
                    {
                        Id = c.Id,
                        CustomerId = c.CustomerId,
                        Amount = c.Amount,
                        DocumentNumber = c.DocumentNumber,                       
                        CreateDate = c.CreateDate,
                    })
                    .ToListAsync();

                return new Result()
                {
                    InterestNotes = data
                };
            }
        }
    }
}
