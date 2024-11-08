﻿using System.IO;

namespace MTCG.HttpServer.Request
{
    public enum HttpMethod
    {
        Options, // required for CORS(sent by swagger)
        Get,
        Post,
        Put,
        Delete,
        Patch
    }

    internal static class MethodUtilities
    {
        public static HttpMethod GetMethod(string method)
        {
            return method.ToLower() switch
            {

                "options" => HttpMethod.Options,
                "get" => HttpMethod.Get,
                "post" => HttpMethod.Post,
                "put" => HttpMethod.Put,
                "delete" => HttpMethod.Delete,
                "patch" => HttpMethod.Patch,
                _ => throw new InvalidDataException()
            };
        }
    }

}
