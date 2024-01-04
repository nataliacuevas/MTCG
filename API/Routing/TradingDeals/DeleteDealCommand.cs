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

namespace MTCG.API.Routing.TradingDeals
{
    internal class DeleteDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly DatabaseStacksDao _stacksDao;
        private readonly DatabaseTradingDealsDao _tradingDealsDao;
        private readonly string _dealId;

        public DeleteDealCommand(DatabaseStacksDao stacksDao, DatabaseTradingDealsDao tradingDealsDao, User user, string dealId)
        {
            _user = user;
            _stacksDao = stacksDao;
            _tradingDealsDao = tradingDealsDao;
            _dealId = dealId;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            string payload;
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            TradingDeal existing_deal = _tradingDealsDao.SelectDealById(_dealId);

            if (existing_deal == null)
            {
                string payload1 = "The provided deal ID was not found";
                response = new HttpResponse(StatusCode.NotFound, payload1);
                return response;
            }

            if (!userCards.Contains(existing_deal.CardToTrade))
            {
                payload = "The deal contains a card that is not owned by the user";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }

            //GOOD REQUEST
            //The deal is deleted and removed from the DB
            _tradingDealsDao.DeleteDeal(_dealId);
            payload = $"Trading deal successfully deleted";
            response = new HttpResponse(StatusCode.Created, payload);
            return response;

        }
    }
}
