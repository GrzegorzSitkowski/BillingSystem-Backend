using Azure.Core;
using BillingSystem.Application.Logic.User;
using BillingSystem.Infrastructure.Auth;
using BillingSystem.WebApi.Application.Auth;
using BillingSystem.WebApi.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BillingSystem.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly CookieSettings? _cookieSettings;
        private readonly JwtManager _jwtManager;
        private readonly IAntiforgery _antiforgery;

        public UsersController(ILogger<UsersController> logger,
            IOptions<CookieSettings> cookieSettings,
            IAntiforgery antiforgery,
            JwtManager jwtManager,
            IMediator mediator) : base(logger, mediator)
        {
            _cookieSettings = cookieSettings != null ? cookieSettings.Value : null;
            _jwtManager = jwtManager;
            _antiforgery = antiforgery;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> CreateUserWithAccount([FromBody] CreateOrUpdateCustomerCommand.Request model)
        {
            var createAccountResult = await _mediator.Send(model);
            var token = _jwtManager.GenerateUserToken(createAccountResult.UserId);
            SetTokenCookie(token);
            return Ok(new JwtToken() { AccessToken = token});
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginCommand.Request model)
        {
            var loginResult = await _mediator.Send(model);
            var token = _jwtManager.GenerateUserToken(loginResult.UserId);
            SetTokenCookie(token);
            return Ok(new JwtToken());// { AccessToken = token });
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            var logoutResult = await _mediator.Send(new CurrentAccountQuery.Request());
            DeleteTokenCookie();
            return Ok(logoutResult);
        }

        [HttpGet]
        public async Task<ActionResult> GetLoggedInUser()
        {
            var data = await _mediator.Send(new CurrentAccountQuery.Request() { });
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> AntiforgeryToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(tokens.RequestToken);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOption = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = SameSiteMode.Lax,
            };

            if(_cookieSettings != null)
            {
                cookieOption = new CookieOptions()
                {
                    HttpOnly = cookieOption.HttpOnly,
                    Expires = cookieOption.Expires,
                    Secure = _cookieSettings.Secure,
                    SameSite = _cookieSettings.SameSite,
                };
            }

            Response.Cookies.Append(CookieSettings.CookieName, token, cookieOption);
        }

        private void DeleteTokenCookie()
        {
            Response.Cookies.Delete(CookieSettings.CookieName, new CookieOptions()
            {
                HttpOnly = true,
            });
        }
    }
}
