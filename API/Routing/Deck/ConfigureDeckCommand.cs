using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System.Collections.Generic;
using System.Linq;

namespace MTCG.API.Routing.Deck
{
    internal class ConfigureDeckCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IStacksDao _stacksDao;
        private readonly List<string> _cardsIds;
        public ConfigureDeckCommand(IStacksDao stacksDao, User user, List<string> cardsIds)
        {
            _stacksDao = stacksDao;
            _user = user;
            _cardsIds = cardsIds;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;

            //Deserialization checks
            if (_cardsIds == null || _cardsIds.Count != 4)
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }

            List<string> allUserCards = _stacksDao.SelectCardsByUsername(_user.Username);

            //here we compare both list of ids and check if the cardsIds are contained in allUserCards
            bool isContained = _cardsIds.All(id => allUserCards.Contains(id));

            if (!isContained)
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
