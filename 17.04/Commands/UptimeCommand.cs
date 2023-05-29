using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04.Commands
{
    internal class UptimeCommand : ICommand
    {
        private Server server;

        public UptimeCommand(Server server)
        {
            this.server = server;
        }

        public List<string> Args => new List<string>();

        public string Exec()
        {
            TimeSpan ts = server.timeStarted.Subtract(DateTime.Now);
            string returnString = "";

            if(ts.Hours > 0)
            {
                returnString += ts.Hours.ToString() + ":";
            }
            else
            {
                returnString += "00:";
            }

            returnString += ts.Minutes.ToString() + ":";
            returnString += ts.Seconds.ToString();

            return returnString;
        }
    }
}
