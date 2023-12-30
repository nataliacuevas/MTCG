using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public int Coins { get; set; }
        public int Elo { get; set; } 
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Token => $"{Username}-mtcgToken";
        public bool IsAdmin => Username == "admin";

        public User(string username, string password, string name, string bio, string image, int coins, int elo, int wins, int losses)
        {
            Username = username;
            Password = password;
            Name = name;
            Bio = bio;
            Image = image;
            Coins = coins;
            Elo = elo;
            Wins = wins;
            Losses = losses;

        }
        
    }
}
