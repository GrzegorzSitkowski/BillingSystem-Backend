using Azure.Core;
using BillingSystem.Application.Logic.User;
using BillingSystem.Infrastructure.Auth;
using BillingSystem.WebApi.Application.Auth;
using BillingSystem.WebApi.Application.Response;
using MediatR;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BillingSystem.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        public CustomerController(ILogger<CustomerController> logger,
            IMediator mediator) : base(logger, mediator)
        {

        }
    }
}
