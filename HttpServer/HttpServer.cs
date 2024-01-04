using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using MTCG.API.Routing;

namespace MTCG.HttpServer
{
    public class HttpServer
    {
        private readonly RequestRouter _router;
        private readonly TcpListener _listener;
        private bool _listening;

        public HttpServer(RequestRouter router, IPAddress address, int port)
        {
            _router = router;
            _listener = new TcpListener(address, port);
            _listening = false;
        }

        public void Start()
        {
            _listener.Start();
            _listening = true;
            Console.WriteLine("Listening incoming connection...");
            while (_listening)
            {   var client = _listener.AcceptTcpClient();
             
                var thready = new Thread(() => {
                    var clientHandler = new HttpClientHandler(client);
                    // Uncomment to verify multithreading
                    // Console.WriteLine("Thread Id: {0}", Thread.CurrentThread.ManagedThreadId.ToString());
                    // Thread.Sleep(5000);
                    HandleClient(clientHandler);
                });
                thready.Start();
                
                Console.WriteLine("Listening incoming connection...");
            }
        }

        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }

        private void HandleClient(HttpClientHandler handler)
        {
            var request = handler.ReceiveRequest();
            HttpResponse response;
            //Request is null if the incoming http request failed for some reason
            if (request is null)
            {
                response = new HttpResponse(StatusCode.BadRequest);
                handler.SendResponse(response);
            }
            else
            {
                try
                {
                    var command = _router.Resolve(request);
                    if (command is null)
                    {
                        response = new HttpResponse(StatusCode.BadRequest);
                    }
                    else
                    {
                        response = command.Execute();
                    }
                }
                catch (RouteNotAuthenticatedException)
                {
                    response = new HttpResponse(StatusCode.Unauthorized);
                }
            }
            try
            {
                handler.SendResponse(response);
            }
            catch (InvalidOperationException)
            {
                //TODO ERASE DEBUG
                // to avoid swagger sending a strange empty message
                // crashing the server
                Console.WriteLine("OOPS");
            }
            
        }
    }
}
