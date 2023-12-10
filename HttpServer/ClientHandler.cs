using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Schemas;

namespace MTCG.HttpServer
{
    internal class ClientHandler
    {
       Socket _clientSocket;
       public ClientHandler(Socket clientSock)
        {
            _clientSocket = clientSock;

            //  _clientSocket.Send(Encoding.ASCII.GetBytes("Hello World"));
            byte[] bufferAsByteArray = new byte[1024];
            int length = clientSock.Receive(bufferAsByteArray);
            string messageReceived = Encoding.ASCII.GetString(bufferAsByteArray, 0, length);

            HttpParser http = new HttpParser(messageReceived);
            HttpResponse reply = new HttpResponse(_clientSocket);
            if (http.Path.Count == 1 && http.Path[0] == "users" && http.Verb == "POST")
            {
                var userCreds = JsonNet.Deserialize<UserCredentials>(http.Body);
                if(userCreds.IsValid())
                {
                    reply.Send(200, "ok");
                    Console.WriteLine("Username: {0} Password: {1}", userCreds.Username, userCreds.Password);

                }
                else
                {
                    reply.Send(400, "dsddf");
                }
            }
            else
            {
                reply.Send(204, "miau");
            }
            
         //   http.Print();


            _clientSocket.Close();
        } 
    }
}
