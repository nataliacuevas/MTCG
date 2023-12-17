﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer.Response
{
    internal class HttpResponse
    {
        public StatusCode StatusCode { get; set; }
        public string Payload { get; set; }

        public HttpResponse(StatusCode statusCode, string payload = null)
        {
            StatusCode = statusCode;
            Payload = payload;
        }
    }
}
