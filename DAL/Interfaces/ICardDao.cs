using MTCG.HttpServer.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Interfaces
{
    public interface ICardDao
    {
        public void CreateCard(Card card);
        public void CreateCards(List<Card> cards);
        public Card GetCardbyId(string id);
        public List<Card> GetCardsByIdList(List<string> ids);
    }
}
