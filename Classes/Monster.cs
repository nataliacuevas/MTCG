using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    class Monster : Card
    {
        public Monster(string name, ElementType type, int damage) : base(name, type, damage) { }
    }
}
