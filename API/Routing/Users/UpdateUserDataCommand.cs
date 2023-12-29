using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using MTCG.DAL;
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
        private readonly DatabaseUserDao _dbUserDao;
        private UserData _userData;
        private User _identity;
        private string _username;

        public UpdateUserDataCommand(DatabaseUserDao dbUserDao, User identity, string username, UserData userData) 

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
