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
        private Stack _stack = new Stack();
        public int Coins { get; }

        public User(string name, string password)
        {
            Name = name;
            _password = password;
            Coins = 20;

        }
        public void AddCardToStack(Card card) 
        {
            _stack.AddCard(card);
        }

        public void Print ()
        {
            Console.WriteLine("user: {0}, pass: {1}, coins: {2} stack: ", Name, _password, Coins);
            _stack.Print();
        }

    }
}
