using MTCG.HttpServer.Schemas;
using System.Collections.Generic;

namespace MTCG.DAL.Interfaces
{
    public interface ITradingsDao
    {
        public List<TradingDeal> GetAllTradingDeals();
        public void CreateDeal(TradingDeal tdeal);
        public TradingDeal SelectDealById(string dealId);
        public void DeleteDeal(string dealId);
        public void DeleteMultipleDealsByCardId(string cardId);
    }
}
