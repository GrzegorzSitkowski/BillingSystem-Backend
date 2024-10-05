using BillingSystem.Application.Logic.Billing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingSystem.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BillingController : BaseController
    {
        public BillingController(ILogger<BillingController> logger,
            IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpGet]
        public async Task<ActionResult> List([FromQuery] ListQuery.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> ListToInterest([FromQuery] ListInvoiceWithInterest.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }
    }
}
