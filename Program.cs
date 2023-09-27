using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MTCG.Classes; //here we connect with the classes folder

namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello, world");
            User user = new User("Nat", "1234");
            user.Print();
            Card card = new Card("carta", ElementType.Water, 5);
            card.Print();
            Stack stack = new Stack();
            stack.AddCard(card);
            Card card2 = new Card("carta", ElementType.Water, 6);
            stack.AddCard(card2);
            Card card3 = new Card("carta", ElementType.Fire, 5);
            stack.AddCard(card3);
            stack.Print();
            Spell spellCard = new Spell("magic", ElementType.Normal, 10);
            stack.AddCard(spellCard);
            stack.Print();



        }
    }
}
