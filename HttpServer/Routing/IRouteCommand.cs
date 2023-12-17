using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Response;

namespace MTCG.HttpServer.Routing
{
    internal interface IRouteCommand
    {
        HttpResponse Execute();
    }
}
