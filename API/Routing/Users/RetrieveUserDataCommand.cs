using Json.Net;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;

namespace MTCG.API.Routing.Users
{
    public class RetrieveUserDataCommand : IRouteCommand
    {
        private readonly IUserDao _dbUserDao;
        private readonly User _user;
        private readonly string _username;
        public RetrieveUserDataCommand(IUserDao dbUserDao, User user, String username)
        {
            _dbUserDao = dbUserDao;
            _user = user;
            _username = username;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;
            // checks that the request comes from the user itself or Admin
            if (_user.Username != _username && !_user.IsAdmin)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
                return response;
            }
            User requestedUser = _dbUserDao.SelectUserByUsername(_username);
            // user requested does not exist
            if (requestedUser == null)
            {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            }

            var userData = new UserData
            {
                Bio = requestedUser.Bio,
                Image = requestedUser.Image,
                Name = requestedUser.Name
            };
            var payload = JsonNet.Serialize(userData);
            response = new HttpResponse(StatusCode.Ok, payload);
            return response;
        }
    }
}
