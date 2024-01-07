using MTCG.HttpServer.Request;


namespace MTCG.HttpServer.Routing
{
    internal interface IRouter
    {
        IRouteCommand Resolve(HttpRequest request);
    }
}
