using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04
{
    internal class UserList
    {
        public List<User> users { get; set; }


        public bool ContainsUsername(string username)
        {
            foreach(User user in users)
            {
                if (user.Username.Equals(username))
                {
                    Console.WriteLine(user.Username + " == "+ username);
                    return true;
                }
            }

            return false;
        }

        public string? GetPasswordForUser(string username)
        {
            foreach (User user in users)
            {
                if (user.Username.Equals(username))
                {
                    return user.Password;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return "Users:"+users.Count+".";
        }
    }
}
