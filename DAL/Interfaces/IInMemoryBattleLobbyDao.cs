namespace MTCG.DAL.Interfaces
{
    public interface IInMemoryBattleLobbyDao
    {
        public string AddToLobby(string username);
        public string Fight(string username1, string username2);
    }
}
