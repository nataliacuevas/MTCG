using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

            http.Print();
            
            _clientSocket.Close();
        } 
    }
}
