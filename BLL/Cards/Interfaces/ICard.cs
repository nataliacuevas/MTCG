using MTCG.Models;

namespace MTCG.Interfaces
{
    public interface ICard
    {
        string Name { get; }
        ElementType Type { get; }
        double Damage { get; }
    }
}
