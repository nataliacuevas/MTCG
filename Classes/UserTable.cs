using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Classes
{
    internal class UserTable
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string  Name { get; set; }
        public string Bio {  get; set; }
        public string Image { get; set; }

        public UserTable(string username, string password, string name, string bio, string image) 
        {
            Username = username;
            Password = password;
            Name = name;
            Bio = bio;
            Image = image;
        } 
    }
}
