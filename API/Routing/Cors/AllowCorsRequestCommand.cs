using MTCG.HttpServer.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Routing;

namespace MTCG.API.Routing.Cors
{
    internal class AllowCorsRequestCommand : IRouteCommand
    {
        public AllowCorsRequestCommand() {        }
        public HttpResponse Execute()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Access-Control-Allow-Origin", "*");
            header.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            header.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

            return new HttpResponse(StatusCode.Ok, header : header);
        
        }
    }
}
