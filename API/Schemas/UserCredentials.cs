﻿namespace MTCG.HttpServer.Schemas
{
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if (Username == null || Password == null)
            {
                return false;
            }
            return true;
        }


    }
}
