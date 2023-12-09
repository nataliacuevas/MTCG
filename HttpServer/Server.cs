using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    internal class Server
    {
        Socket _serverSock;
        public Server() 
        {

            _serverSock = new Socket(AddressFamily.InterNetwork, //IPV4
                                     SocketType.Stream, //Stream sends byte array data
                                     ProtocolType.Tcp); //TCP

            //Bind socket to IP Address and Port
            _serverSock.Bind(new IPEndPoint(IPAddress.Loopback, 10001));

            while (true)
            {
                _serverSock.Listen(1000);
                Console.WriteLine("Listening to incoming connections");
                Socket clientSock = _serverSock.Accept();

                ClientHandler client = new ClientHandler(clientSock);
            } 
            
        }
        ~Server() 
        {
            _serverSock.Close();
        }
    }
}
