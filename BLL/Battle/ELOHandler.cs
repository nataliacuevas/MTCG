using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;


namespace MTCG.Classes
{
    public static class ELOHandler
    {
        public static void Update(Players? winner, User user1, User user2) 
        {
            if (winner == Players.PlayerA)
            {
                user1.Elo += 3;
                user2.Elo -= 5;

                user1.Wins += 1;
                user2.Losses += 1;
            }
            else if (winner == Players.PlayerB)
            {
                user2.Elo += 3;
                user1.Elo -= 5;

                user2.Wins += 1;
                user1.Losses += 1;
            }
            //else nothing changes 
        }
    }
}
