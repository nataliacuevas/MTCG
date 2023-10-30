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
            Deck deckA = new Deck();
            Deck deckB = new Deck();

            User playerA = new User("Nat", "1234", 100, deckA);
            User playerB = new User("Pancho", "12345", 100, deckB); 



            Monster card1 = new Monster("Knight1", ElementType.Water, 1, MonsterType.Knight);
            Monster card2 = new Monster("Elf1", ElementType.Water, 2, MonsterType.Elf);
            Monster card3 = new Monster("Kraken1", ElementType.Water, 3, MonsterType.Kraken);
            Monster card4 = new Monster("Goblin1", ElementType.Fire, 4, MonsterType.Goblin);
            Monster card5 = new Monster("Wizard1", ElementType.Fire, 5, MonsterType.Wizard);
            Monster card6 = new Monster("Dragon1", ElementType.Normal, 6, MonsterType.Dragon);
            Monster card7 = new Monster("Orc1", ElementType.Normal, 7, MonsterType.Orc);

            Spell card8 = new Spell("Fireball1", ElementType.Fire, 8);
            Spell card9 = new Spell("Lightning1", ElementType.Normal, 9);
            Spell card10 = new Spell("Splash", ElementType.Water, 10);
            Spell card11 = new Spell("Blast1", ElementType.Fire, 11);
            Spell card12 = new Spell("Freeze1", ElementType.Water, 12);

            Monster card13 = new Monster("Knight2", ElementType.Normal, 13, MonsterType.Knight);
            Monster card14 = new Monster("Elf2", ElementType.Water, 14, MonsterType.Elf);
            Monster card15 = new Monster("Kraken2", ElementType.Fire, 15, MonsterType.Kraken);
            Monster card16 = new Monster("Goblin2", ElementType.Normal, 16, MonsterType.Goblin);
            Monster card17 = new Monster("Wizard2", ElementType.Water, 17, MonsterType.Wizard);
            Monster card18 = new Monster("Dragon2", ElementType.Fire, 18, MonsterType.Dragon);
            Monster card19 = new Monster("Orc2", ElementType.Normal, 19, MonsterType.Orc);

            Spell card20 = new Spell("Fireball2", ElementType.Fire, 20);

            //add Monster card
            playerB.AddCardToDeck(card1);
            playerB.AddCardToDeck(card2);
            playerA.AddCardToDeck(card3);
            playerA.AddCardToDeck(card4);

            //add spell card 
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
