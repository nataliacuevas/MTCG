using MTCG.HttpServer.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
