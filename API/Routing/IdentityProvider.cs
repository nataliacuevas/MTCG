using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Request;
using MTCG.Models;

namespace MTCG.API.Routing
{
    // Class that handles the authentication by bearer token for all the requests 
    public class IdentityProvider
    {
        private readonly IUserDao _databaseUserDao;

        public IdentityProvider(IUserDao databaseUserDao)
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
                        // if not found current user remains null
                    }
                }
            }

            return currentUser;
        }
    }
}
