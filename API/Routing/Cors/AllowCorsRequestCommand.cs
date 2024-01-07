using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using System.Collections.Generic;

namespace MTCG.API.Routing.Cors
{
    /* Class implemented to allow Cors request for Swagger. Ultimately did not work, pending to fix
     */
    internal class AllowCorsRequestCommand : IRouteCommand
    {
        public AllowCorsRequestCommand() { }
        public HttpResponse Execute()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Access-Control-Allow-Origin", "*");
            header.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            header.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");

            return new HttpResponse(StatusCode.Ok, header: header);

        }
    }
}
