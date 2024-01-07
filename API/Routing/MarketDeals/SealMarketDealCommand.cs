using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.API.Routing.MarketDeals
{
    public class SealMarketDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IUserDao _userDao;
        private readonly IStacksDao _stacksDao;
        private readonly ITradingsDao _tradingDealsDao;
        private readonly IMarketDao _marketDealsDao;
        private readonly string _dealId;

        public SealMarketDealCommand(IStacksDao stacksDao, ITradingsDao tradingDealsDao, IMarketDao databaseMarketDealsDao, IUserDao databaseUserDao, User user, string dealId)
        {
            _userDao = databaseUserDao;
            _user = user;
            _stacksDao = stacksDao;
            _tradingDealsDao = tradingDealsDao;
            _dealId = dealId;
            _marketDealsDao = databaseMarketDealsDao;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;
            string payload;
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            //Makes sure that the deals exists
            MarketDeal deal = _marketDealsDao.SelectMarketDealById(_dealId);
            if (deal == null)
            {
                payload = "The provided market deal ID was not found\n";
                response = new HttpResponse(StatusCode.NotFound, payload);
                return response;
            }
            if (userCards.Contains(deal.CardToSell))
            {
                payload = "the user tried to sell a card to themselves \n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }
            if (_user.Coins < deal.Price)
            {
                payload = "the deal requirements are not met (Price)\n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }

            //GOOD REQUEST
            //Accept Market Deal and exchange the card for money
            string dealOwner = _stacksDao.GetCardOwnerbyCardId(deal.CardToSell);
            User deal_owner = _userDao.SelectUserByUsername(dealOwner);

            /* I did not find another way to cast this nullable value to int. I am 100% sure that it is not null. 
             Required for the addition operator "+"
            deal.price is nullable for the JSON.Net library, to detect missing keys from the schema
            */
            int dealPrice = deal.Price ?? 0;
            _userDao.UpdateUserCoins(deal_owner, deal_owner.Coins + dealPrice);
            _userDao.UpdateUserCoins(_user, _user.Coins - dealPrice);

            _stacksDao.UpdateCardOwnership(deal.CardToSell, _user.Username);
            //Delete ALL possible deals associated with the offered card, as is not available anymore
            _tradingDealsDao.DeleteMultipleDealsByCardId(deal.CardToSell);
            _marketDealsDao.DeleteMarketDeal(deal.Id);
            response = new HttpResponse(StatusCode.Ok);
            return response;

        }
    }
}
