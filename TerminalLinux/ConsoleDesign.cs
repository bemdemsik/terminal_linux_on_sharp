using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    public static class ConsoleDesign
    {
        public static string Start()
        {
            Console.Write("[" + Environment.UserName + "@" + Environment.UserDomainName + "]\n[" + Directory.GetCurrentDirectory() + "] $ ");
            return Console.ReadLine();
        }
    }
}
