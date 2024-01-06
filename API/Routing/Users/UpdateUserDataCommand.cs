using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using MTCG.DAL;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.Models;
using MTCG.HttpServer.Schemas;
using MTCG.API.Routing;
using MTCG.BLL;
using MTCG.HttpServer.Routing;

namespace MTCG.API.Routing.Users
{
    internal class UpdateUserDataCommand : IRouteCommand
    {
        private readonly IUserDao _dbUserDao;
        private readonly UserData _userData;
        private readonly User _identity;
        private readonly string _username;

        public UpdateUserDataCommand(IUserDao dbUserDao, User identity, string username, UserData userData) 

        {
            _dbUserDao = dbUserDao;
            _identity = identity;
            _userData = userData;
            _username = username;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            if(_identity.Username != _username)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
                return response;
            }

            try
            {
                _dbUserDao.UpdateUserData(_userData, _identity);
                response = new HttpResponse(StatusCode.Ok);
            }
            catch (UserNotFoundException)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }
}
