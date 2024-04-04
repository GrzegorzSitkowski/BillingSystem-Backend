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

namespace BillingSystem.WebApi.Application.Auth
{
    public static class JwtAuthenticationDataProviderConfiguration
    {
        public static IServiceCollection AddJwtAuthenticationDataProvider(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<CookieSettings>(configuration.GetSection("CookieSettings"));
            services.AddScoped<IAuthenticationDataProvider, JwtAuthenticationDataProvider>();

            return services;
        }
    }
}
