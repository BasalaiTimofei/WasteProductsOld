using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace WasteProducts.Web.ExceptionHandling.Exceptions
{
    public class WasteException : Exception
    {
        public HttpStatusCode Code { get; set; }

        public string Message { get; set; }

        public WasteException(string message, HttpStatusCode code) : this (message)
        {
            Code = code;
        }

        public WasteException() : base ()
        {
        }

        public WasteException(string message) : base (message)
        {
            Message = message;
        }

        public WasteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public WasteException(string message, Exception inner) : base (message, inner)
        {
        }

        
    }
}