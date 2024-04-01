using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Exceptions
{
    public class ErrorExceptions
    {
        public string Error { get; private set; }

        public ErrorExceptions(string error) 
        {
            Error = error;
        }

    }
}
