using BillingSystem.Application.Interfaces;
using BillingSystem.Infrastructure.Auth;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Infrastructure.Persistence
{
    public static class JwtAuthConfiguration
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<JwtAuthenticationOptions>(configuration.GetSection("JwtAuthentication"));

            return services;
        }
    }
}
