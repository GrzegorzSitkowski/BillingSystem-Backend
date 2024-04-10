using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Validators
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TRequest>
    {
        private readonly IList<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators.ToList();
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(v => v.Errors)
                .Where(e => e != null)
                .GroupBy(e => new { e.PropertyName, e.ErrorCode})
                .ToList();

            if (errors.Any())
            {
                throw new Exceptions.ValidationException()
                {
                    Errors = errors.Select(e => new Exceptions.ValidationException.FieldError()
                    {
                        Error = e.Key.ErrorCode,
                        FieldName = e.Key.PropertyName,
                    }).ToList(),
                };
            }

            return await next();
        }
    }
}
