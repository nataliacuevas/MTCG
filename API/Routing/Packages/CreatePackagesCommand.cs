﻿using MTCG.DAL;
using MTCG.DAL.Interfaces;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.API.Routing.Packages;
using MTCG.HttpServer.Schemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;
using MTCG.Interfaces;

namespace MTCG.API.Routing.Packages
{
    internal class CreatePackagesCommand :IRouteCommand
    {

        private readonly IPackagesDao _dbPackagesDao;
        private readonly ICardDao _cardDao;
        private readonly User _user;
        private readonly List<Card> _cards;


        public CreatePackagesCommand(ICardDao cardDao, IPackagesDao databasePackagesDao, User user, List<Card> packagesContent)
        {
            _cardDao = cardDao;
            _user = user;
            _dbPackagesDao = databasePackagesDao;
            _user = user;
            _cards = packagesContent;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;
            //check is the user is authorized (admin)
            if(!_user.IsAdmin)
            {
                response = new HttpResponse(StatusCode.Unauthorized);
                return response;
            }
            //check that evry card is valid
            foreach(Card card in _cards)
            {
                if (!card.IsValid())
                {
                    response = new HttpResponse(StatusCode.BadRequest);
                    return response;
                }
                if(_cardDao.GetCardbyId(card.Id) != null) 
                {
                    string payload = $"Card with ID: {card.Id} already stored in DB\n";
                    response = new HttpResponse(StatusCode.Conflict, payload:payload);
                    return response;
                }
            }
            //check correct number of cards
            if(_cards.Count != 5)
            {
                response = new HttpResponse(StatusCode.BadRequest);
                return response;
            }
            //GOOD REQUEST
            // each card is created in the cards DB
            _cardDao.CreateCards(_cards);
            _dbPackagesDao.CreatePackage(_cards);

            return new HttpResponse(StatusCode.Ok);



            /*
            try
            {
                _dbPackagesDao.CreatePackages(_cardDao);

                response = new HttpResponse(StatusCode.Created);
            }
            //Other way to check if the user is already in DB
            catch (DuplicateNameException)
            {
                response = new HttpResponse(StatusCode.Conflict);
            }

            return response;
            */
        }

    }
    
}
