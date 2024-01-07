using MTCG.HttpServer.Response;

namespace MTCG.HttpServer.Routing
{
    public interface IRouteCommand
    {
        HttpResponse Execute();
    }
}
