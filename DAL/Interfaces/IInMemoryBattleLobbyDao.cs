using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Interfaces
{
    public interface IInMemoryBattleLobbyDao
    {
        public string AddToLobby(string username);
        public string Fight(string username1, string username2);
    }
}
