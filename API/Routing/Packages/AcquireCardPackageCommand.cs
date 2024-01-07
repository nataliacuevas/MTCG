using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;

namespace MTCG.API.Routing.Packages
{
    public class AcquireCardPackageCommand : IRouteCommand
    {
        private readonly IPackagesDao _dbPackagesDao;
        private readonly IStacksDao _stacksDao;
        private readonly IUserDao _userDao;
        private readonly User _user;
        private readonly int _packagePrice = 5;

        public AcquireCardPackageCommand(IPackagesDao databasePackagesDao, IStacksDao stacksDao, IUserDao userDao, User user)
        {
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
                response = new HttpResponse(StatusCode.Forbidden, payload: payload);
                return response;
            }
            //GOOD REQUEST
            Package randomPackage = _dbPackagesDao.PopRandomPackage();
            if (randomPackage == null)
            {
                string payload = "No card package available for buying";
                response = new HttpResponse(StatusCode.NotFound, payload: payload);
                return response;
            }

            _stacksDao.AddPackageToUser(_user, randomPackage);

            _userDao.UpdateUserCoins(_user, _user.Coins - _packagePrice);

            return new HttpResponse(StatusCode.Ok);
        }
    }
}
