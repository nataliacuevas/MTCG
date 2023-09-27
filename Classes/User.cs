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

        public User(string name, string password)
        {
            Name = name;
            _password = password;

        }
        public void addCardToStack(Card card) 
        {
            _stack.AddCard(card);
        }

        public void Print ()
        {
            Console.WriteLine("user: {0}, pass: {1}, stack: ", Name, _password);
            _stack.Print();
        }

    }
}
