using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;

namespace MTCG.API.Routing.Users
{
    public class LoginCommand : IRouteCommand
    {
        private readonly IUserDao _dbUserDao;
        private readonly UserCredentials _credentials;

        public LoginCommand(IUserDao dbUserDao, UserCredentials credentials)
        {
            _credentials = credentials;
            _dbUserDao = dbUserDao;
        }

        public HttpResponse Execute()
        {
            User user = _dbUserDao.LoginUser(_credentials);
            HttpResponse response;
            //if login fails 
            if (user == null)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
            }
            else
            {
                response = new HttpResponse(StatusCode.Ok, user.Token);
            }

            return response;
        }

    }
}
