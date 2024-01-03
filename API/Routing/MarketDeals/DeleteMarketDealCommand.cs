using MTCG.DAL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.MarketDeals
{
    internal class DeleteMarketDealCommand: IRouteCommand
    {
        private readonly User _user;
        private readonly DatabaseStacksDao _stacksDao;
        private readonly DatabaseMarketDealsDao _marketDealsDao;
        private readonly string _dealId;

        public DeleteMarketDealCommand(DatabaseStacksDao stacksDao, DatabaseMarketDealsDao databaseMarketDealsDao, User user, string dealId)
        {
            _user = user;
            _stacksDao = stacksDao;
            _dealId = dealId;
            _marketDealsDao = databaseMarketDealsDao;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            string payload;
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            MarketDeal existing_deal = _marketDealsDao.SelectMarketDealById(_dealId);
            if (!userCards.Contains(existing_deal.CardToSell))
            {
                payload = "The deal contains a card that is not owned by the user";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }
            if (existing_deal.Id == null)
            {
                string payload1 = "The provided market deal ID was not found";
                response = new HttpResponse(StatusCode.NotFound, payload1);
                return response;
            }
            //GOOD REQUEST
            //The deal is deleted and removed from the DB
            _marketDealsDao.DeleteMarketDeal(_dealId);
            payload = $"Market deal successfully deleted";
            response = new HttpResponse(StatusCode.Created, payload);
            return response;

        }
    }
}
