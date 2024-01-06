using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.DAL;

namespace MTCG.DAL.Interfaces
{
    public interface IUserDao
    {
        public User SelectUserByUsername(string Username);
        public void CreateUser(UserCredentials userCredentials);
        public IEnumerable<User> GetAllUsers();
        public User GetUserByAuthToken(string authToken);
        public void UpdateUserData(UserData userdata, User user);
        public void UpdateUserCoins(User user, int newValue);
        public User LoginUser(UserCredentials credentials);
        public void UpdateEloWinsLosses(User user);

    }
}
