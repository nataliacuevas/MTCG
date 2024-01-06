using MTCG.HttpServer.Schemas;
using MTCG.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
