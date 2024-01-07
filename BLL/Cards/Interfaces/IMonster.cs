using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Interfaces
{
    public interface IMonster : ICard
    {
        MonsterType Mtype { get; }
    }
}
