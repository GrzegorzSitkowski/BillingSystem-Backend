using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Infrastructure.Auth
{
    public class JwtAuthenticationOptions
    {
        public string? Secret { get; set; } //hasło do szyfrowania
        public string? Issuer { get; set; } // dostawca tokena
        public string? Audience { get; set; } //odbiorca tokena
        public int ExpireInDays { get; set; } = 30;
    }
}
