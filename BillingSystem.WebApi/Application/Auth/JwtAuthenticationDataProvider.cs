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
            throw new NotImplementedException();
        }
    }
}
