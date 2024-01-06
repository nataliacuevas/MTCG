using MTCG.DAL;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //Here we check if the price is non-positive
            if (!_deal.IsValid())
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            List<string> cardsInDeck = _stacksDao.SelectCardsInDeckByUsername(_user.Username);
            if (!userCards.Contains(_deal.CardToSell) || cardsInDeck.Contains(_deal.CardToSell))
            {
                if (!userCards.Contains(_deal.CardToSell)) { payload = "card not in stack"; }
                else { payload = "card in configured deck"; }
                response = new HttpResponse(StatusCode.Forbidden, payload);
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
