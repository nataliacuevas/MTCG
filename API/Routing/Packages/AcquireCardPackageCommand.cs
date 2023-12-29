using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.API.Routing;
using MTCG.DAL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Interfaces;
using MTCG.Models;

namespace MTCG.API.Routing.Packages
{
    internal class AcquireCardPackageCommand : IRouteCommand
    {
        private readonly DatabasePackagesDao _dbPackagesDao;
        private DatabaseCardDao _cardDao;
        private DatabaseStacksDao _stacksDao;
        private DatabaseUserDao _userDao;
        private readonly User _user;

        public AcquireCardPackageCommand(DatabaseCardDao cardDao, DatabasePackagesDao databasePackagesDao, DatabaseStacksDao stacksDao, DatabaseUserDao userDao, User user)
        {
            _cardDao = cardDao;
            _user = user;
            _dbPackagesDao = databasePackagesDao;
            _userDao = userDao;
            _stacksDao = stacksDao;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            if (_user.Coins < 5)
            {
                string payload = "Not enough money for buying a card package";
                response = new HttpResponse(StatusCode.Forbidden, payload:payload);
                return response;
            }

            //WHAT IF THERE ARE NO MORE PACKAGES!!!!
            //GOOD REQUEST
            Package randomPackage = _dbPackagesDao.PopRandomPackage();
            if(randomPackage == null)
            {
                string payload = "No card package available for buying";
                response = new HttpResponse(StatusCode.NotFound, payload:payload);
                return response;
            }
            _stacksDao.AddPackageToUser(_user, randomPackage);

            //TODO REMOVE COINS FROM USERS TABLES AFTER THE PURCHASE
            _userDao.UpdateUserCoins(_user, _user.Coins-5);

            return new HttpResponse(StatusCode.Ok);
        }
    }
}
