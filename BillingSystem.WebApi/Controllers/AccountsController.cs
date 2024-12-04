using Azure.Core;
using BillingSystem.Application.Logic.Account;
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
    public class AccountsController : BaseController
    {
        public AccountsController(ILogger<AccountsController> logger,
            IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentAccount()
        {
            var data = await _mediator.Send(new CurrentAccountQuery.Request() { });
            return Ok(data);
        }
    }
}
