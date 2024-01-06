using MTCG.DAL;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.TradingDeals
{
    public class SealDealCommand : IRouteCommand
    {
        private readonly User _user;
        private readonly IStacksDao _stacksDao;
        private readonly ITradingsDao _tradingDealsDao;
        private readonly ICardDao _cardDao;
        private readonly string _dealId;
        private readonly string _offeredCardId;  

        public SealDealCommand(ICardDao cardDao, IStacksDao stacksDao, ITradingsDao tradingDealsDao, User user, string dealId, string offeredCardId)
        {
            _cardDao = cardDao;
            _user = user;
            _stacksDao = stacksDao;
            _tradingDealsDao = tradingDealsDao;
            _dealId = dealId;
            _offeredCardId = offeredCardId;
        }
        public HttpResponse Execute()
        {
            HttpResponse response;
            string payload;
            List<string> userCards = _stacksDao.SelectCardsByUsername(_user.Username);
            TradingDeal deal = _tradingDealsDao.SelectDealById(_dealId);
            if (deal == null)
            {
                payload = "The provided deal ID was not found\n";
                response = new HttpResponse(StatusCode.NotFound, payload);
                return response;
            }
            if (userCards.Contains(deal.CardToTrade))
            {
                payload = "the user tried to trade with self\n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }
            if (!userCards.Contains(_offeredCardId))
            {
                payload = "The offered card is not owned by the user\n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }
            Card offeredCard = _cardDao.GetCardbyId(_offeredCardId);
            if(offeredCard == null)
            {
                payload = "The offered card does not exist\n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }
            List<string> cardsInDeck = _stacksDao.SelectCardsInDeckByUsername(_user.Username);
            if (cardsInDeck.Contains(offeredCard.Id))
            {
                payload = "the offered card is locked in the deck\n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }
            if(offeredCard.Damage < deal.MinimumDamage || offeredCard.GetCardType() != deal.Type)
            {
                payload = "the deal requirements are not met (Type, MinimumDamage)\n";
                response = new HttpResponse(StatusCode.Forbidden, payload);
                return response;
            }

            //GOOD REQUEST
            //Accept Deal and switch cards
            string dealOwner = _stacksDao.GetCardOwnerbyCardId(deal.CardToTrade);
            _stacksDao.UpdateCardOwnership(offeredCard.Id, dealOwner);
            _stacksDao.UpdateCardOwnership(deal.CardToTrade, _user.Username);
            //Delete ALL deals associated with the traded cards, as they changed owners 
            _tradingDealsDao.DeleteMultipleDealsByCardId(offeredCard.Id);
            _tradingDealsDao.DeleteMultipleDealsByCardId(deal.CardToTrade);
            response = new HttpResponse(StatusCode.Ok);
            return response;

        }

    }
}
