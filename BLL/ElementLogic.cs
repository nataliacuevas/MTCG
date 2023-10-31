using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    public static class ElementLogic
    {
        public static double ElementModifier(ElementType dis, ElementType other)
        {
            //Fire & water cases
            if (dis == ElementType.Water && other == ElementType.Fire)
            {
                return 2;
            }
            else if (dis == ElementType.Fire && other == ElementType.Water)
            {
                return 1.0 / 2.0;
            }
            // Fire & Normal cases
            else if (dis == ElementType.Fire && other == ElementType.Normal)
            {
                return 2;
            }
            else if (dis == ElementType.Normal && other == ElementType.Fire)
            {
                return 1.0 / 2.0;
            }
            //Normal & water cases
            else if (dis == ElementType.Normal && other == ElementType.Water)
            {
                return 2;
            }
            else if (dis == ElementType.Water && other == ElementType.Normal)
            {
                return 1.0 / 2.0;
            }
            else
            {
                return 1;
            }
        }
    }
}
