using BillingSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Logic.Abstractions
{
    public abstract class BaseQueryHandler
    {
        protected readonly ICurrentAccountProvider _currentAccountProvider;
        protected readonly IApplicationDbContext _applicationDbContext;

        protected BaseQueryHandler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext)
        {
            _currentAccountProvider = currentAccountProvider;
            _applicationDbContext = applicationDbContext;
        }
    }
}
