using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Application.Logic.InterestNotes
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

                var model = await _applicationDbContext.InterestNotes.FirstOrDefaultAsync(c => c.Id == request.Id && c.CreatedBy == account.Id);
                var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Id == model.CustomerId);

                if (model == null)
                {
                    throw new UnauthorizedAccessException();
                }

                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "InterestNote.txt"), true))
                {
                    outputFile.WriteLine($"Number of customer: {model.CustomerId}");
                    outputFile.WriteLine('\n');
                    outputFile.WriteLine($"{customer.FullName}");
                    outputFile.WriteLine($"{customer.Address}");
                    outputFile.WriteLine($"{customer.PostCode}");
                    outputFile.WriteLine($"{customer.City}");
                    outputFile.WriteLine($"{customer.Email}");
                    outputFile.WriteLine('\n');
  
                    outputFile.WriteLine($"Value of interest: {model.Amount} zł");
                    outputFile.WriteLine($"Number of document: {model.DocumentNumber}");
                    outputFile.WriteLine($"Document with interest: {model.InvoiceNumber}");
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
