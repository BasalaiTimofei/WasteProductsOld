using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WasteProducts.Web.ExceptionHandling.Exceptions
{
    public class WasteException : Exception
    {
        public HttpStatusCode Code { get; set; }

        public string Message { get; set; }

        public WasteException(string message, HttpStatusCode code) : base (message)
        {
            Message = message;
            Code = code;
        }
    }
}