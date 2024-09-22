using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Application.Logic.Invoices
{
    public static class Print
    {
        public class Request : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
        {
            public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
            {
            }

            public async Task<Result> Handle(Request request,  CancellationToken cancellationToken)
            {
                var account = await _currentAccountProvider.GetAuthenticatedAccount();

                var model = await _applicationDbContext.Invoices.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);
                var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == model.CustomerId);

                if(model == null)
                {
                    throw new UnauthorizedAccessException();
                }

                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "invoice.txt"), true))
                {
                    outputFile.WriteLine($"Number of customer: {model.CustomerId}");
                    outputFile.WriteLine('\n');
                    outputFile.WriteLine($"{model.CustomerName}");
                    outputFile.WriteLine($"{customer.Address}");
                    outputFile.WriteLine($"{customer.PostCode}");
                    outputFile.WriteLine($"{customer.City}");
                    outputFile.WriteLine($"{customer.Email}");
                    outputFile.WriteLine('\n');
  
                    outputFile.WriteLine($"Value of invoice: {model.Amount} zł");
                    outputFile.WriteLine($"Due date: {model.DueDate}");
                    outputFile.WriteLine($"Your balance: {customer.Balance}");
                    outputFile.WriteLine('\n');
                    outputFile.WriteLine($"Please pay {model.Amount}zł until {model.DueDate} for our bank account XXX XXXX XXXXXX XXXX XX");
                }

                return new Result()
                {
                    Id = model.Id
                };          
            }
        }
    }
}
