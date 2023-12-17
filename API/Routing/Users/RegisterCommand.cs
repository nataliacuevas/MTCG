using MTCG.DAL;
//using MTCG.HttpServer;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.HttpServer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MTCG.API.Routing.Users
{
    internal class RegisterCommand : IRouteCommand
    {

    
        private readonly DatabaseUserDao _dbUserDao;
        private readonly UserCredentials _credentials;

        public RegisterCommand(DatabaseUserDao dbUserDao, UserCredentials credentials)
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
            if(_dbUserDao.SelectUserByUsername(_credentials.Username) != null)
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
