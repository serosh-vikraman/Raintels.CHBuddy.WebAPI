using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Raintels.CHBuddy.Web.API
{
    public class ResponseDataModel<T> where T : class
    {
        public HttpStatusCode Status { get; set; }
        public T Response { get; set; }
        public string Message { get; set; }
        public ErrorResponse ErrorMessage { get; set; }
    }

    public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
