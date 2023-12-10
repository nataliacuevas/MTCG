using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Json.Net;


namespace MTCG.HttpServer.Schemas
{
    internal class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if(Username == null || Password == null) 
            {
                return false;
            }
            return true;
        }


    }
}
