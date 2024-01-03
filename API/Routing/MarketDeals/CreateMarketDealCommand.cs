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

namespace MTCG.API.Routing.MarketDeals
{
    internal class CreateMarketDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly DatabaseStacksDao _stacksDao;
        private readonly DatabaseTradingDealsDao _tradingDealsDao;
        private readonly DatabaseMarketDealsDao _marketDealsDao;
        private readonly MarketDeal _deal;

        public CreateMarketDealCommand(DatabaseStacksDao stacksDao, DatabaseTradingDealsDao tradingDealsDao, DatabaseMarketDealsDao databaseMarketDealsDao, User user, MarketDeal deal)
        {
            _user = user;
            _stacksDao = stacksDao;
            _tradingDealsDao = tradingDealsDao;
            _marketDealsDao = databaseMarketDealsDao;
            _deal = deal;

        }
        public HttpResponse Execute()
        {
            HttpResponse response;
            if (!_deal.IsValid())
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }
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
                string payload1 = $"Marker Deal with ID: {_deal.Id} already stored in DB\n";
                response = new HttpResponse(StatusCode.Conflict, payload1);
                return response;
            }
            //GOOD REQUEST
            //The deal is created and stored in the DB
            _marketDealsDao.CreateDeal(_deal);
            string payload = $"Market deal successfully created\n";
            response = new HttpResponse(StatusCode.Created, payload);
            return response;

        }
    }
}
