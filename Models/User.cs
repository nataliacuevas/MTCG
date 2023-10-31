using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Classes;
using MTCG.BLL;

namespace MTCG.Models
{
    public class User
    {
        public string Name { get; }
        private string _password;
        public Deck UserDeck { get; set; }
        public int Coins { get; }
        public int ELO { get; set; }

        public User(string name, string password, int elo, Deck userDeck)
        {
            Name = name;
            _password = password;
            Coins = 20;
            ELO = elo;
            UserDeck = userDeck;


        }
        public void AddCardToDeck(CardLogic card) 
        {
            UserDeck.AddCard(card);
        }

        public void Print ()
        {
            Console.WriteLine("user: {0}, pass: {1}, coins: {2} deck: ELOHandler: {3} ", Name, _password, Coins, ELO);
            UserDeck.Print();
        }

    }
}
