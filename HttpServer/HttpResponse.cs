using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    internal class HttpResponse
    {
        Socket _sock;

        public HttpResponse(Socket sock)
        {
            _sock = sock;
        }
        public void Send(int code, string body)
        {
            string header = "";

            switch(code)
            {
                case 200:
                    header = "HTTP/1.1 200 OK\r\n";
                    break;
                case 201:
                    header = "HTTP/1.1 201 User successfully created\r\n";
                    break;
                case 204:
                    header = "HTTP/1.1 204 No Content\r\n";
                    break;
                case 400:
                    header = "HTTP/1.1 400 Bad Request\r\n";
                    break;
                case 401:
                    header = "HTTP/1.1 401 Unauthorized\r\n";
                    break;
                case 403:
                    header = "HTTP/1.1 403 Forbidden\r\n";
                    break;
                case 404:
                    header = "HTTP/1.1 404 Not Found\r\n";
                    break;
                case 409:
                    header = "HTTP/1.1 409 User with same username already registered\r\n";
                    break;
                default:
                    throw new ArgumentException("Code case " + code.ToString() + " not valid");
            }
            //TODO DATE 
            //TODO SERVER
            //TODO LAST MODIFIED
            //header += "Content-Length: " + body.Length.ToString() + "\r\n";
            header += "Content-Type: text/html\r\n";
            header += "Connection: Closed\r\n";
            string fullMessage = header + "\r\n" + body;

            //Convert string to a byte array
            byte[] bytesOfMessage = Encoding.ASCII.GetBytes(fullMessage);

            _sock.Send(bytesOfMessage);
        }       

    }
}
