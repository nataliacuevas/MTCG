using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer.Request
{
    public class HttpRequest
    {

        public HttpMethod Method { get; set; }
        public string ResourcePath { get; set; }
        public string HttpVersion { get; set; }
        public Dictionary<string, string> Header { get; set; }
        public string Payload { get; set; }

        public HttpRequest(HttpMethod method, string resourcePath, string httpVersion, Dictionary<string, string> header, string payload = null)
        {
            Method = method;
            ResourcePath = resourcePath;
            HttpVersion = httpVersion;
            Header = header;
            Payload = payload;
        }
    }
}
