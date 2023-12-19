using MTCG.BLL;
using MTCG.DAL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Users
{
    internal class LoginCommand : IRouteCommand
    {
        private readonly DatabaseUserDao _dbUserDao;
        private readonly UserCredentials _credentials;

        public LoginCommand(DatabaseUserDao dbUserDao, UserCredentials credentials)
        {
            _credentials = credentials;
            _dbUserDao = dbUserDao;
        }

        public HttpResponse Execute()
        {
            User user;
            try
            {
                user = _dbUserDao.LoginUser(_credentials);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            HttpResponse response;
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
