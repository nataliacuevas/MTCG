using MTCG.Models;
using System.Collections.Generic;

namespace MTCG.DAL.Interfaces
{
    public interface IStacksDao
    {
        public void AddPackageToUser(User user, Package package);
        public List<string> SelectCardsByUsername(string username);
        public void ClearDeck(string username);
        public List<string> SelectCardsInDeckByUsername(string username);
        public void PutInDeck(List<string> cardsIds);
        public void UpdateCardOwnership(string cardId, string newOwner);
        public string GetCardOwnerbyCardId(string cardId);

    }
}
