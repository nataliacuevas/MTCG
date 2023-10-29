using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    class User
    {
        public string Name { get; }
        private string _password;
        private Deck _deck = new Deck();
        public int Coins { get; }
        public int ELO { get; set; }

        public User(string name, string password, int elo)
        {
            Name = name;
            _password = password;
            Coins = 20;
            ELO = elo;


        }
        public void AddCardToDeck(Card card) 
        {
            _deck.AddCard(card);
        }

        public void Print ()
        {
            Console.WriteLine("user: {0}, pass: {1}, coins: {2} deck: ", Name, _password, Coins);
            _deck.Print();
        }

    }
}
