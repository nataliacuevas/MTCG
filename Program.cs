using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MTCG.Classes;
using MTCG.Models;
using MTCG.BLL;

//here we connect with the classes folder


namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Deck deckA = new Deck();
            Deck deckB = new Deck();

            User playerA = new User("Nat", "1234", 100, deckA);
            User playerB = new User("Pancho", "12345", 100, deckB); 



            MonsterLogic card1 = new MonsterLogic("Knight1", ElementType.Water, 1, MonsterType.Knight);
            MonsterLogic card2 = new MonsterLogic("Elf1", ElementType.Water, 2, MonsterType.Elf);
            MonsterLogic card3 = new MonsterLogic("Kraken1", ElementType.Water, 3, MonsterType.Kraken);
            MonsterLogic card4 = new MonsterLogic("Goblin1", ElementType.Fire, 4, MonsterType.Goblin);
            MonsterLogic card5 = new MonsterLogic("Wizard1", ElementType.Fire, 5, MonsterType.Wizard);
            MonsterLogic card6 = new MonsterLogic("Dragon1", ElementType.Normal, 6, MonsterType.Dragon);
            MonsterLogic card7 = new MonsterLogic("Orc1", ElementType.Normal, 7, MonsterType.Orc);

            SpellLogic card8 = new SpellLogic("Fireball1", ElementType.Fire, 8);
            SpellLogic card9 = new SpellLogic("Lightning1", ElementType.Normal, 9);
            SpellLogic card10 = new SpellLogic("Splash", ElementType.Water, 10);
            SpellLogic card11 = new SpellLogic("Blast1", ElementType.Fire, 11);
            SpellLogic card12 = new SpellLogic("Freeze1", ElementType.Water, 12);

            MonsterLogic card13 = new MonsterLogic("Knight2", ElementType.Normal, 13, MonsterType.Knight);
            MonsterLogic card14 = new MonsterLogic("Elf2", ElementType.Water, 14, MonsterType.Elf);
            MonsterLogic card15 = new MonsterLogic("Kraken2", ElementType.Fire, 15, MonsterType.Kraken);
            MonsterLogic card16 = new MonsterLogic("Goblin2", ElementType.Normal, 16, MonsterType.Goblin);
            MonsterLogic card17 = new MonsterLogic("Wizard2", ElementType.Water, 17, MonsterType.Wizard);
            MonsterLogic card18 = new MonsterLogic("Dragon2", ElementType.Fire, 18, MonsterType.Dragon);
            MonsterLogic card19 = new MonsterLogic("Orc2", ElementType.Normal, 19, MonsterType.Orc);

            SpellLogic card20 = new SpellLogic("Fireball2", ElementType.Fire, 20);

            //add MonsterLogic card
            playerB.AddCardToDeck(card1);
            playerB.AddCardToDeck(card2);
            playerA.AddCardToDeck(card3);
            playerA.AddCardToDeck(card4);

            //add SpellLogic card 
            playerB.AddCardToDeck(card8);
            playerB.AddCardToDeck(card9);
            playerA.AddCardToDeck(card10);
            playerA.AddCardToDeck(card11);

            playerA.Print();
            playerB.Print();

            Battle fight = new Battle();

            var (log, winner)  = fight.Match(playerA.UserDeck , playerB.UserDeck);
            log += "\n" + fight.FinalMatchText(winner);
            ELOHandler.Update(winner, playerA, playerB);

            Console.WriteLine(log);
            playerA.Print();
            playerB.Print();

        }
    }
}
