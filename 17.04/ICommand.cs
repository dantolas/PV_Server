using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17._04
{
    internal interface ICommand
    {

        List<string> Args { get; }
        string Exec();


    }
}
