using MediatR;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BillingSystem.Application.Logic.Payments;

namespace BillingSystem.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        public PaymentsController(ILogger<PaymentsController> logger,
            IMediator mediator) : base(logger, mediator)
        {

        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCommand.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] DeleteCommand.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetPayment([FromQuery] GetQuery.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> List([FromQuery] ListQuery.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }
    }
}
