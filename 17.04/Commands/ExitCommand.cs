using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04.Commands
{
    internal class ExitCommand : ICommand
    {
        private Server server;

        public ExitCommand(Server server)
        {
            this.server = server;
        }

        public List<string> Args => new List<string>();

        public string Exec()
        {
            return "";
        }
    }
}
