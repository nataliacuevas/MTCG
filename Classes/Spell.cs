﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    class Spell : Card
    {
        public Spell(string name, ElementType type, int damage) : base(name, type, damage) { }

        public override void Print()
        {
            Console.WriteLine("Spell Card Name: {0}, Element Type:  {1}, Damage: {2}", Name, Type, Damage);
        }
    }
}
