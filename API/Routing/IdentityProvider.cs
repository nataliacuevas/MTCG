using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.HttpServer.Request;

namespace MTCG.API.Routing
{
    public class IdentityProvider
    {
        private readonly DatabaseUserDao _databaseUserDao;

        public IdentityProvider(DatabaseUserDao databaseUserDao)
        {
            _databaseUserDao = databaseUserDao;
        }
        //if user not found, returns null
        public User GetIdentityForRequest(HttpRequest request)
        {
            User currentUser = null;

            if (request.Header.TryGetValue("Authorization", out var authToken))
            {
                const string prefix = "Bearer ";
                if (authToken.StartsWith(prefix))
                {
                    try
                    {
                        currentUser = _databaseUserDao.GetUserByAuthToken(authToken.Substring(prefix.Length));
                    }
                    catch 
                    {
                        Console.WriteLine("Failed to identify user by Auth Token");
                    }
                }
            }

            return currentUser;
        }
    }
}
