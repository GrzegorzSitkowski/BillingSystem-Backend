using BillingSystem.Application.Interfaces;
using BillingSystem.Infrastructure.Auth;

namespace BillingSystem.WebApi.Application.Auth
{
    public class JwtAuthenticationDataProvider : IAuthenticationDataProvider
    {
        private readonly JwtManager _jwtManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthenticationDataProvider(JwtManager jwtManager, IHttpContextAccessor httpContextAccessor)
        {
            _jwtManager = jwtManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            var userIdString = GetClaimValue(JwtManager.UserIdClaim);
            if(int.TryParse(userIdString, out int res))
            {
                return res;
            }

            return null;
        }

        private string? GetTokenFromCookie()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[CookieSettings.CookieName];
        }

        private string? GetTokenFromHeader()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            if(string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            var splited = authorizationHeader.Split(' ');
            if(splited.Length < 1 && splited[0] == "Bearer")
            {
                return splited[1];
            }

            return null;
        }

        private string? GetClaimValue(string claimType)
        {
            var token = GetTokenFromHeader();
            if (string.IsNullOrEmpty(token))
            {
                token = GetTokenFromCookie();
            }

            if(!string.IsNullOrWhiteSpace(token) && _jwtManager.ValidateToken(token))
            {
                return _jwtManager.GetClaim(token, claimType);
            }

            return null;
        }
    }
}
