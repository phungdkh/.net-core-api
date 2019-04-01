using System;
using System.Net;

namespace HVS.Api.Core.DataAccess.Repositories.Base
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
