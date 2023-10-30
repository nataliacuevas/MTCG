using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MTCG.Classes
{
    public static class ELOHandler
    {
        public static void Update(Players? winner, User player1, User player2) 
        {
            if (winner == Players.PlayerA)
            {
                player1.ELO += 3;
                player2.ELO -= 5;
            }
            else if (winner == Players.PlayerB)
            {
                player2.ELO += 3;
                player1.ELO -= 5;
            }
            //else nothing changes 
        }
    }
}
