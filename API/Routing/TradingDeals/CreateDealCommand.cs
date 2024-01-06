using MTCG.BLL;
using MTCG.DAL;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Interfaces;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.TradingDeals
{
    public class CreateDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IStacksDao _stacksDao;
        private readonly ITradingsDao _tradingDealsDao;
        private readonly TradingDeal _deal;

        public CreateDealCommand(IStacksDao stacksDao, ITradingsDao tradingDealsDao, User user, TradingDeal deal)
        {
            _user = user;
            _stacksDao = stacksDao;
            _tradingDealsDao = tradingDealsDao;
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
            if (!userCards.Contains(_deal.CardToTrade) || cardsInDeck.Contains(_deal.CardToTrade))
            {
                response = new HttpResponse(StatusCode.Forbidden);
                return response;
            }
            TradingDeal exist_already = _tradingDealsDao.SelectDealById(_deal.Id);
            if (exist_already != null)
            {
                string payload1 = $"Deal with ID: {_deal.Id} already stored in DB\n";
                response = new HttpResponse(StatusCode.Conflict, payload1);
                return response;
            }
            //GOOD REQUEST
            //The deal is created and stored in the DB
            _tradingDealsDao.CreateDeal(_deal);
            string payload = $"Trading deal successfully created\n";
            response = new HttpResponse(StatusCode.Created, payload);
            return response;
           
        }
    }
}
