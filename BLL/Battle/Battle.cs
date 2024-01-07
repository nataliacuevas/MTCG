using MTCG.BLL;
using MTCG.BLL.Cards;

namespace MTCG.Classes
{
    // Enum used to indicate who is a winner in a round and match
    public enum Players
    {
        PlayerA,
        PlayerB,
    }
    public class Battle
    {
        private const int _maxRounds = 100;

        public Battle()
        {
        }
        // Returns a tuple consisting of the round log, and the winner (null if draw)
        public (string, Players?) Round(CardLogic cardOne, CardLogic cardTwo, string userA, string userB)
        {

            //using deconstruction to get the output results
            // the damange modifier produces a description string that is included in the logs.
            var (description1, dmg1) = cardOne.DamageModifier(cardTwo);

            var (description2, dmg2) = cardTwo.DamageModifier(cardOne);

            string log = userA + ": " + cardOne.Type + cardOne.Name + "(" + cardOne.Damage + ") vs ";
            log += userB + ": " + cardTwo.Type + cardTwo.Name + "(" + cardTwo.Damage + ") \n    -> ";
            log += description1 + description2 + " -> " + dmg1.ToString() + " vs " + dmg2.ToString() + "\n    -> ";

            // If PlayerA wins
            if (dmg1 > dmg2)
            {
                return (log + cardOne.Name + " wins. (" + userA + ")", Players.PlayerA);
            }
            // If PlayerB wins
            else if (dmg1 < dmg2)
            {
                return (log + cardTwo.Name + " wins. (" + userB + ")", Players.PlayerB);
            }
            // Draw
            else
            {
                return (log + " Draw. ", null);
            }
        }

        public (string, Players?) Match(Deck deckA, Deck deckB, string userA, string userB)
        {
            string finaLog = "";
            int roundCounter = 0;


            do
            {

                // Pops a random card from deck, and repeats until finding a "non potion" card.
                CardLogic cardA = deckA.PopRandomCard();
                CardLogic modCardA = cardA;
                while (cardA is PotionLogic && deckA.Count > 0)
                {
                    PotionLogic PotionCardA = (PotionLogic)cardA;
                    cardA = deckA.PopRandomCard();
                    modCardA = PotionCardA.ApplyPotion(cardA);
                }
                // Here either modCardA is a potion (no more cards left), or a modified non-potion card.
                // Repeats for the deckB
                CardLogic cardB = deckB.PopRandomCard();
                CardLogic modCardB = cardB;
                while (cardB is PotionLogic && deckB.Count > 0)
                {
                    PotionLogic PotionCardB = (PotionLogic)cardB;
                    cardB = deckB.PopRandomCard();
                    modCardB = PotionCardB.ApplyPotion(cardB);
                }


                var (log, winner) = Round(modCardA, modCardB, userA, userB);
                finaLog += "\n" + log;
                ++roundCounter;

                // The non-potion cards that were taken out are given to the winning player.
                // The match ends if the other player has no cards.
                if (winner == Players.PlayerA)
                {
                    deckA.AddCard(cardA);
                    deckA.AddCard(cardB);

                    if (deckB.Count == 0)
                    {

                        return (finaLog, Players.PlayerA);
                    }
                }
                else if (winner == Players.PlayerB)
                {
                    deckB.AddCard(cardA);
                    deckB.AddCard(cardB);

                    if (deckA.Count == 0)
                    {
                        return (finaLog, Players.PlayerB);
                    }
                }
                else
                {
                    //draw Case
                    deckA.AddCard(cardA);
                    deckB.AddCard(cardB);
                }
            }
            while (roundCounter < _maxRounds);
            // return null to indicate a draw
            return (finaLog, null);
        }
        // text appended at the end of the logs.
        public string FinalMatchText(Players? winner, string userA, string userB)
        {
            if (winner == null)
            {
                return "Draw, there is no winner";
            }
            else
            {
                string winnerUser;
                if (winner == Players.PlayerA)
                {
                    winnerUser = userA;
                }
                else
                {
                    winnerUser = userB;
                }
                return "The winner of the Battle is " + winnerUser;
            }
        }
    }
}
