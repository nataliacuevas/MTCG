using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Print();

        }
        public void Print()
        {

            Console.WriteLine($"StatusCode: {StatusCode}");

            if(Header != null)
            {
                foreach (var head in Header)
                {
                    Console.WriteLine($"Header: {head.Key} : {head.Value}");
                }
            }
            if(Payload != null)
            {
                Console.WriteLine($"Payload: {Payload}");

            }
        }
    }
}
