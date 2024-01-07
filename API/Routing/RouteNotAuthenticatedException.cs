using System;

namespace MTCG.HttpServer
{
    // This exception is thrown by the GetIdentity function to indicate that the user is not authenticated
    public class RouteNotAuthenticatedException : Exception
    {
        public RouteNotAuthenticatedException() { }
    }
}
