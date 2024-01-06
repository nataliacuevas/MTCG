using MTCG.BLL;
using MTCG.DAL.Interfaces;
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
