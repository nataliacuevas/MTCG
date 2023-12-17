using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API
{
    internal class TokenHandler
    {
        public static string GetTokenByUsername(string username)
        {
            return username + "-mtcgToken";
        }

        public static bool VerifyUsernameByToken(string token, string username) 
        {
            if (token == username + "-mtcgToken")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
