using Json.Net;
using Microsoft.SqlServer.Server;
using MTCG.DAL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.Deck
{
    internal class ConfigureDeckCommand : IRouteCommand
    {
        private readonly User _user;
        private DatabaseStacksDao _stacksDao;
        private List<string> _cardsIds;
        public ConfigureDeckCommand(DatabaseStacksDao stacksDao, User user, List<string> cardsIds)
        {
            _stacksDao = stacksDao;
            _user = user;
            _cardsIds = cardsIds;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;

            //SECURITY CHECKS
            if(_cardsIds == null || _cardsIds.Count != 4)
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }

            List<string> allUserCards = _stacksDao.SelectCardsByUsername(_user.Username);

            //here we compare both list of ids and check if the cardsIds are contained in allUserCards
            bool isContained = _cardsIds.All(id => allUserCards.Contains(id));

            if(!isContained)
            {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }
            _stacksDao.ClearDeck(_user.Username);
            _stacksDao.PutInDeck(_cardsIds);

            response = new HttpResponse(StatusCode.Ok);
            return response;
        }
    }
}
