using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string massage = null)
        {
            StatusCode = statusCode;
            Massage = massage ?? GetDefaultMassageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Massage { get; set; }


        private string GetDefaultMassageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You have made a bad request!",
                401 => "You are not Authorized!",
                404 => "Resource is not found!",
                500 => "Internal Server Error ! ",
                _ => null
            };
        }

    }
}
