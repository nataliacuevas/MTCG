using System.Collections.Generic;

namespace MTCG.HttpServer.Response
{
    public class HttpResponse
    {
        public StatusCode StatusCode { get; set; }
        public string Payload { get; set; }
        public Dictionary<string, string> Header { get; set; }

        public HttpResponse(StatusCode statusCode, string payload = null, Dictionary<string, string> header = null)
        {
            StatusCode = statusCode;
            Payload = payload;
            Header = header;
        }
    }
}
