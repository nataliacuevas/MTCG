using MTCG.Models;

namespace MTCG.BLL
{
    //This class implements the method used to apply damage modifiers based on elements. 
    //The variable name "dis" is used as a replacement of the protected keyword "This" 
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
