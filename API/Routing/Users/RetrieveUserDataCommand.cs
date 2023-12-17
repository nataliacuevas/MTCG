using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Json.Net;
using MTCG.DAL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;

namespace MTCG.API.Routing.Users


{
    internal class RetrieveUserDataCommand : IRouteCommand
    {
        private readonly DatabaseUserDao _dbUserDao;
        private readonly User _user;
        private readonly string _username;
        public RetrieveUserDataCommand(DatabaseUserDao dbUserDao, User user, String username) 
        {
            _dbUserDao = dbUserDao;
            _user = user;
            _username = username;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;
            if (_user.Username != _username && !_user.IsAdmin)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
                return response;
            }
            User requestedUser = _dbUserDao.SelectUserByUsername(_username);
            if (requestedUser == null)
            {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            }

            var userData = new UserData {Bio = requestedUser.Bio,
                                         Image = requestedUser.Image,
                                         Name = requestedUser.Name
                                        };
            var payload = JsonNet.Serialize(userData);
            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }
}
