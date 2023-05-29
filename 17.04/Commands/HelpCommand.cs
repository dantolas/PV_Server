using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04.Commands
{
    internal class HelpCommand : ICommand
    {
        public List<string> Args => new List<string>();

        public string Exec()
        {
            return "Time -> Shows current date and time." + "\n"
                + "Who -> Prints information about the user connected to server." + "\n"
                + "Uptime -> Runtime of the server." + "\n"
                + "Stats -> Amount of users connected, failed connections and commands executed." + "\n"
                + "Last -> Last time a user was connected." + "\n"
                + "Exit -> Logout.";
        }
    }
}
