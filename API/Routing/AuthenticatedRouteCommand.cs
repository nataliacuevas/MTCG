using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing
{
    public abstract class AuthenticatedRouteCommand : IRouteCommand
    {
        public User Identity { get; private set; }
        protected AuthenticatedRouteCommand(User identity)
        {
            Identity = identity;
        }

        public abstract HttpResponse Execute();
    }
}
