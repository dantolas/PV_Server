using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04.Commands
{
    internal class TimeCommand : ICommand
    {
        public List<string> Args => new List<string>();

 
        public string Exec()
        {
            return DateTime.Now.ToString();
        }

      
    }
}
