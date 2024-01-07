using Json.Net;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.MarketDeals
{
    public class SelectAllMarketDealsCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IMarketDao _marketDealsDao;

        public SelectAllMarketDealsCommand(IMarketDao databaseMarketDealsDao, User user)
        {
            //user only used for the GetIdentity function
            // this is for authentication
            _user = user;
            _marketDealsDao = databaseMarketDealsDao;
        }
        public HttpResponse Execute()
        {
            string payload;
            HttpResponse response;
            List<MarketDeal> deals = _marketDealsDao.GetAllMarketDeals();

            if (deals.Count == 0)
            {
                payload = "There are no Market Deals available";
                response = new HttpResponse(StatusCode.NoContent, payload);
                return response;
            }
            else
            {
                payload = JsonNet.Serialize(deals);
                response = new HttpResponse(StatusCode.Ok, payload);
                return response;
            }
        }
    }
}
