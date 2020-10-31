using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string massage = null, string details = null) : base(statusCode, massage)
        {
            Details = details;
        }

        public string  Details { get; set; }
    }
}
