﻿using Npgsql.Internal.TypeHandlers.GeometricHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.HttpServer
{
    internal class HttpParser
    {
        public bool IsValid { get; private set; }
        public string Path { get; private set; } = string.Empty;
        public string Verb { get; private set; } = string.Empty;
        public string Protocol { get; private set; } = string.Empty;
        public Dictionary<string, string> Header { get; private set; }
        public string Body { get; private set; } = string.Empty;
        public string RawMessage { get; }

        void SetInvalid(string reason)
        {
            Path = string.Empty;
            Verb = string.Empty;
            Protocol = string.Empty;
            Body = string.Empty;
            IsValid = false;

            Console.WriteLine("Request rejected because: {0}", reason);
            Console.WriteLine("Raw Message: \n{0}", RawMessage);

        }
        public HttpParser(string message) 
        {
            RawMessage = message;
            message = message.Replace("\r\n", "\n");
            IsValid = true;

            string[] lines = message.Split('\n');
            if(lines.Length == 0) 
            {
                SetInvalid("Message is empty");
                return;
            }
    
            string Title = lines[0];
            string[] Words = Title.Split(' ');
            if(Words.Length != 3) 
            {
                SetInvalid("Header is not well formed");
                return; 
            }
            Verb = Words[0];
            Path = Words[1];
            Protocol = Words[2];
            string[] protocolParts = Protocol.Split('/');
            if(protocolParts.Length != 2) 
            {
                SetInvalid("Protocol not well formed");
                return;
            }
            if (protocolParts[0] != "HTTP")
            {
                SetInvalid("Not an HTTP method");
                return;
            }
            int index = 0;
            Header = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                if(line == string.Empty)
                {
                    break;
                }
                if(index > 0)
                {
                    int colonIndex = line.IndexOf(':');
                    if(colonIndex == -1)
                    {
                        SetInvalid("Missing separator for dictionary (HEADER)");
                        return;
                    }
                    string key = line.Substring(0, colonIndex);
                    string value = line.Substring(colonIndex + 2);
                    Header.Add(key, value);

                }

                ++index;
            }
            for(int i = index+1; i < lines.Length; ++i)
            {
                Body += lines[i] + "\n";
            }
            if(Body.Length != 0)
            {
                Body = Body.Remove(Body.Length - 1);
            }
            
        }
        public void Print()
        {
            Console.WriteLine("Is Valid: {0}", IsValid);
            Console.WriteLine("Path: {0}", Path);
            Console.WriteLine("Verb: {0}", Verb);
            Console.WriteLine("Protocol: {0}", Protocol);
            foreach(var kvp in Header)
            {
                Console.WriteLine("Header Key: {0} Header Value: {1}", kvp.Key, kvp.Value);
            }
            Console.WriteLine("Body: {0}", Body);
        }

    }
}