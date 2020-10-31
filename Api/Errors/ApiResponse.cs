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
                400 => "A bad request, you have made",
                401 => "Authorized , you are not",
                404 => "Resource found , it was not",
                500 => "Errors are the path to dark side. Errors lead to anger. Anger leads to Hate.",
                _ => null
            };
        }

    }
}
