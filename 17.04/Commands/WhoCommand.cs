using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04.Commands
{
    internal class WhoCommand : ICommand
    {
        private Server server;

        public WhoCommand(Server server   )
        {
            this.server = server;
        }

        public List<string> Args => new List<string>();

        public string Exec()
        {
            string returnString = "";

            foreach(string user in this.server.connectedUsers)
            {
                returnString += user + " | ";
            }

            //server.connectedUsers.ForEach(user =>
            //{
            //    returnString += user + " | ";
            //});
            return returnString;
        }
    }
}
