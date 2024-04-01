using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Application.Exceptions
{
    public class ErrorException : Exception
    {
        public string Error { get; private set; }

        public ErrorException(string error) 
        {
            Error = error;
        }

    }
}
