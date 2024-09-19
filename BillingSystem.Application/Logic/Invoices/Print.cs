using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Logic.Invoices
{
    public static class Print
    {
        public class Request
        {
            public int? Id { get; set; }
        }

        public class Handler : BaseQueryHandler
        {
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {
            }

            public async Task Handle(Request request,  CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Invoices.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);

                if(model == null)
                {
                    throw new UnauthorizedAccessException();
                }

                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "invoice.txt"), true))
                {
                    outputFile.WriteLine($"Number of customer: {model.CustomerId}");
                    outputFile.WriteLine($"Name: {model.CustomerName}");
                    outputFile.WriteLine($"Amount: {model.Amount}");
                    outputFile.WriteLine($"Due date: {model.DueDate}");
                }
            }
        }
    }
}
