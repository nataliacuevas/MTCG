using MTCG.DAL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.API.Routing.MarketDeals
{
    internal class SealMarketDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly DatabaseUserDao _userDao;
        private readonly DatabaseStacksDao _stacksDao;
        private readonly DatabaseTradingDealsDao _tradingDealsDao;
        private readonly DatabaseCardDao _cardDao;
        private readonly DatabaseMarketDealsDao _marketDealsDao;
        private readonly string _dealId;
        private readonly Mutex _dealsMutex;

        public SealMarketDealCommand(DatabaseCardDao cardDao, DatabaseStacksDao stacksDao, DatabaseTradingDealsDao tradingDealsDao, DatabaseMarketDealsDao databaseMarketDealsDao, DatabaseUserDao databaseUserDao, User user, string dealId, Mutex dealsmutex)
        {
            _cardDao = cardDao;
            _userDao = databaseUserDao;
            _user = user;
            _stacksDao = stacksDao;
            _tradingDealsDao = tradingDealsDao;
            _dealId = dealId;
            _marketDealsDao = databaseMarketDealsDao;
            _dealsMutex = dealsmutex;
        }
        public HttpResponse Execute()
        {
            _dealsMutex.WaitOne();
            HttpResponse response;
            string payload;
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            MarketDeal deal = _marketDealsDao.SelectMarketDealById(_dealId);
            if (deal.Id == null)
            {
                payload = "The provided market deal ID was not found\n";
                response = new HttpResponse(StatusCode.NotFound, payload);
                _dealsMutex.ReleaseMutex();
                return response;
            }
            if (userCards.Contains(deal.CardToSell))
            {
                payload = "the user tried to sell a card to themselves \n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                _dealsMutex.ReleaseMutex();
                return response;
            }
            if (_user.Coins < deal.Price)
            {
                payload = "the deal requirements are not met (Price)\n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                _dealsMutex.ReleaseMutex();
                return response;
            }

            //GOOD REQUEST
            //Accept Market Deal and exchange the card for money
            string dealOwner = _stacksDao.GetCardOwnerbyCardId(deal.CardToSell);
            User deal_owner = _userDao.SelectUserByUsername(dealOwner);

            // I am completely sure that the value is not null here. Required for the addition +
            int dealPrice = deal.Price ?? 0;
            _userDao.UpdateUserCoins(deal_owner, deal_owner.Coins + dealPrice);
            _userDao.UpdateUserCoins(_user, _user.Coins - dealPrice);

            _stacksDao.UpdateCardOwnership(deal.CardToSell, _user.Username);
            //Delete ALL possible deals associated with the offered card, as is not available anymore
            _tradingDealsDao.DeleteMultipleDealsByCardId(deal.CardToSell);
            _marketDealsDao.DeleteMarketDeal(deal.Id);
            response = new HttpResponse(StatusCode.Ok);
            _dealsMutex.ReleaseMutex();
            return response;

        }
    }
}
