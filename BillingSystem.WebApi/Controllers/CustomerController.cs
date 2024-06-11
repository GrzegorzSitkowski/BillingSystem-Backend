using BillingSystem.Application.Logic.Customer;
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

        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate([FromBody] CreateOrUpdateCustomerCommand.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }
    }
}
