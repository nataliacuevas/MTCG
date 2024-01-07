using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Response;

namespace MTCG.HttpServer.Routing
{
    public interface IRouteCommand
    {
        HttpResponse Execute();
    }
}
