using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using System.Data;

namespace MTCG.API.Routing.Users
{
    public class RegisterCommand : IRouteCommand
    {


        private readonly IUserDao _dbUserDao;
        private readonly UserCredentials _credentials;

        public RegisterCommand(IUserDao dbUserDao, UserCredentials credentials)
        {
            _dbUserDao = dbUserDao;
            _credentials = credentials;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            if (!_credentials.IsValid())
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }
            // user already exists
            if (_dbUserDao.SelectUserByUsername(_credentials.Username) != null)
            {
                return new HttpResponse(StatusCode.Conflict);
            }
            try
            {
                _dbUserDao.CreateUser(_credentials);

                response = new HttpResponse(StatusCode.Created);
            }
            //Other way to check if the user is already in DB
            catch (DuplicateNameException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
        }
    }
}
