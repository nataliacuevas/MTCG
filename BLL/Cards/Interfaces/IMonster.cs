using MTCG.Models;

namespace MTCG.Interfaces
{
    public interface IMonster : ICard
    {
        MonsterType Mtype { get; }
    }
}
