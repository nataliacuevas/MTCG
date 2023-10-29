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
            User user = new User("Nat", "1234");
            user.Print();
            Monster card = new Monster("carta", ElementType.Water, 1, MonsterType.Knight);
           // card.Print();
            Stack stack = new Stack();

            Spell card2 = new Spell("carta", ElementType.Water, 6);
            
            Monster card3 = new Monster("carta", ElementType.Fire, 5, MonsterType.Goblin);
  
            Spell spellCard = new Spell("magic", ElementType.Normal, 10);

            user.AddCardToStack(card);
            user.AddCardToStack(card2);
            user.AddCardToStack(card3);
            user.AddCardToStack(spellCard);

            user.Print();

            Battle fight = new Battle();

            RoundLog log = fight.Round(spellCard, card2);
            Console.WriteLine(log.Log);
        }
    }
}
