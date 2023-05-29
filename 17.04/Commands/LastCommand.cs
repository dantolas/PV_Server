using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04.Commands
{
    internal class LastCommand : ICommand
    {
        public List<string> Args => new List<string>();

     

        public string Exec()
        {
            string returnString = "";
            Args.ForEach(username =>
            {
                returnString += getLastLog(username);
            });
            return returnString;
        }

        private string getLastLog(string username)
        {
            //Do stuff
            return "";
        }

    }
}
