﻿using MTCG.HttpServer.Request;
using MTCG.HttpServer.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    public class HttpClientHandler
    {
        public enum ParseState
        {
            Base,
            Headers,
            Payload,
            Finished
        }

        private readonly TcpClient _client;

        public HttpClientHandler(TcpClient client)
        {
            _client = client;
        }

        public HttpRequest ReceiveRequest()
        {
            try
            {
                // "using" keyword -> immediately dispose and when going out of scope, leaving the socket stream open for sending the response
                //IN CASE OF ERROR check here 
                using var reader = new StreamReader(_client.GetStream(),encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: true);

                Request.HttpMethod method = Request.HttpMethod.Get;
                string path = null;
                string version = null;
                Dictionary<string, string> header = new();
                int contentLength = 0;
                string payload = null;

                ParseState state = ParseState.Base;
                string line;

                while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                {   
                    line = line.Trim();
                    switch (state)
                    {
                        case ParseState.Base:
                            var baseInfo = line.Split(' ');
                            if (baseInfo.Length != 3)
                            {
                                throw new InvalidDataException();
                            }
                            method = MethodUtilities.GetMethod(baseInfo[0]);
                            path = baseInfo[1];
                            version = baseInfo[2];

                            state = ParseState.Headers;
                            break;

                        case ParseState.Headers:
                            int colonIndex = line.IndexOf(':');
                            if (colonIndex == -1)
                            {
                                throw new InvalidDataException();
                            }
                            string key = line.Substring(0, colonIndex).Trim();
                            string value = line.Substring(colonIndex + 1).Trim();
                            header.Add(key, value);

                            // special handling for content length, we need this for reading the payload
                            if (key == "Content-Length")
                            {
                                try
                                {
                                    contentLength = int.Parse(value);
                                }
                                catch (Exception e)
                                {
                                    throw new InvalidDataException("invalid content length", e);
                                }
                            }
                            break;
                    }
                }

                // needed this to tell the compiler that the nullables are not null in the following code
                if (path is null || version is null)
                {
                    return null;
                }

                // check whether we need a payload step
                state = contentLength > 0 && header.ContainsKey("Content-Type") ? ParseState.Payload : ParseState.Finished;

                if (state == ParseState.Payload)
                {
                    // in a more complete implementation, we should consider the content type (i.e. for receiving binary data)
                    // we however only cover textual data

                    var buffer = new char[contentLength];
                    var bytesReadTotal = reader.ReadBlock(buffer, 0, contentLength);

                    if (bytesReadTotal != contentLength)
                    {
                        throw new InvalidDataException();
                    }

                    payload = new string(buffer);
                    state = ParseState.Finished;
                }

                return state == ParseState.Finished ? new HttpRequest(method, path, version, header, payload) : null;
            }
            catch (Exception e) when (e is IOException || e is InvalidDataException)
            {
                return null;
            }
        }

        public void SendResponse(HttpResponse response)
        {
            // https://stackoverflow.com/questions/5757290/http-header-line-break-style

            // "using" keyword -> immediately dispose and close stream when going out of scope
            
            using var writer = new StreamWriter(_client.GetStream());

            writer.Write($"HTTP/1.1 {(int)response.StatusCode} {response.StatusCode}\r\n");
            if(response.Header != null)
            {
                foreach(var header in response.Header)
                {
                    writer.Write($"{header.Key}: {header.Value}\r\n");
                }
            }
            if (!string.IsNullOrEmpty(response.Payload))
            {
                var payload = Encoding.UTF8.GetBytes(response.Payload);
                writer.Write($"Content-Length: {payload.Length}\r\n");
                writer.Write("\r\n");
                writer.Write(Encoding.UTF8.GetString(payload));
            }
            else
            {
                writer.Write("\r\n");
            }
            Console.WriteLine("Response sent");
        }
    }
}
