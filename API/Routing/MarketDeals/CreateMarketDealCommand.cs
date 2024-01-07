using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.MarketDeals
{
    public class CreateMarketDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IStacksDao _stacksDao;
        private readonly IMarketDao _marketDealsDao;
        private readonly MarketDeal _deal;

        public CreateMarketDealCommand(IStacksDao stacksDao, IMarketDao databaseMarketDealsDao, User user, MarketDeal deal)
        {
            _user = user;
            _stacksDao = stacksDao;
            _marketDealsDao = databaseMarketDealsDao;
            _deal = deal;

        }
        public HttpResponse Execute()
        {
            string payload;
            HttpResponse response;
            // check if the price is non-positive and the format is right
            if (!_deal.IsValid())
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }
            // checking that the card is in the stack and not in the configured deck 
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            List<string> cardsInDeck = _stacksDao.SelectCardsInDeckByUsername(_user.Username);
            if (!userCards.Contains(_deal.CardToSell) || cardsInDeck.Contains(_deal.CardToSell))
            {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }
            MarketDeal exist_already = _marketDealsDao.SelectMarketDealById(_deal.Id);
            if (exist_already != null)
            {
                payload = $"Marker Deal with ID: {_deal.Id} already stored in DB\n";
                response = new HttpResponse(StatusCode.Conflict, payload);
                return response;
            }
            //GOOD REQUEST
            //The deal is created and stored in the DB
            _marketDealsDao.CreateDeal(_deal);
            payload = $"Market deal successfully created\n";
            response = new HttpResponse(StatusCode.Created, payload);
            return response;

        }
    }
}
