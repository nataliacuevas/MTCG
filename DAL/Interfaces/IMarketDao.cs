using MTCG.HttpServer.Schemas;
using System.Collections.Generic;

namespace MTCG.DAL.Interfaces
{
    public interface IMarketDao
    {
        public List<MarketDeal> GetAllMarketDeals();
        public void CreateDeal(MarketDeal tdeal);
        public MarketDeal SelectMarketDealById(string dealId);
        public void DeleteMarketDeal(string dealId);
    }
}
