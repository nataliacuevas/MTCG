using MTCG.HttpServer.Request;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing
{
    public interface IIdentityProvider
    {
        public User GetIdentityForRequest(HttpRequest request);
    }
}
