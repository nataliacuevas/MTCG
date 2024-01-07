using Json.Net;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.TradingDeals
{
    public class SelectAllDealsCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly ITradingsDao _tradingDealsDao;

        public SelectAllDealsCommand(ITradingsDao databaseTradingDealsDao, User user)
        {
            //user only used for the GetIdentity function
            _user = user;
            _tradingDealsDao = databaseTradingDealsDao;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;
            List<TradingDeal> deals = _tradingDealsDao.GetAllTradingDeals();

            if (deals.Count == 0)
            {
                response = new HttpResponse(StatusCode.NoContent);
                return response;
            }
            //Good request
            else
            {
                var payload = JsonNet.Serialize(deals);
                response = new HttpResponse(StatusCode.Ok, payload);
                return response;
            }
        }
    }
}
