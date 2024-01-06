using Json.Net;
using MTCG.DAL;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Schemas;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //401 ERROR covered by GetIdentity? 
            else
            {
                var payload = JsonNet.Serialize(deals);
                response = new HttpResponse(StatusCode.Ok, payload);
                return response;
            }
        }
    }
}
