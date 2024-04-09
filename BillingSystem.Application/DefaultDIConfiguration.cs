using BillingSystem.Application.Interfaces;
using BillingSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application
{
    public static  class DefaultDIConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentAccountProvider, CurrentAccountProvider>();

            return services;
        } 
    }
}
