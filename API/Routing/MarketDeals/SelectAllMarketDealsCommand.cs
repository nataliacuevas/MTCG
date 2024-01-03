using Json.Net;
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
    internal class SelectAllMarketDealsCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly DatabaseMarketDealsDao _marketDealsDao;

        public SelectAllMarketDealsCommand(DatabaseMarketDealsDao databaseMarketDealsDao, User user)
        {
            //user only used for the GetIdentity function
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
            //401 ERROR covered by GetIdentity
            else
            {
                payload = JsonNet.Serialize(deals);
                response = new HttpResponse(StatusCode.Ok, payload);
                return response;
            }
        }
    }
}
