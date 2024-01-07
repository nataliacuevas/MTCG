using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Interfaces
{
    public interface ICard
    {
        string Name { get; }
        ElementType Type { get; }
        double Damage { get; }
    }
}
