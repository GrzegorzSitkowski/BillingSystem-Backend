using MediatR;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BillingSystem.Application.Logic.InterestNotes;

namespace BillingSystem.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class InterestNotesController : BaseController
    {
        public InterestNotesController(ILogger<InterestNotesController> logger,
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
        public async Task<ActionResult> GetInterestNote([FromQuery] GetQuery.Request model)
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

        [HttpGet]
        public async Task<ActionResult> PrintInterestNote([FromQuery] Print.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }
    }
}
