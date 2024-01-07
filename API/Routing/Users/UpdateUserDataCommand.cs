using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;

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
            // From the API specification is not clear if the Admin has this right, but I decided that it made sense for the admin to have it
            if (_identity.Username != _username && !_identity.IsAdmin)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
                return response;
            }
            if (_dbUserDao.SelectUserByUsername(_username) == null)
            {
                response = new HttpResponse(StatusCode.NotFound);
                return response;
            }
            // Good Request
            _dbUserDao.UpdateUserData(_userData, _identity);
            response = new HttpResponse(StatusCode.Ok);

            return response;
        }
    }
}
