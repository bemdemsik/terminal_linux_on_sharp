using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class Exit
    {
        public static void ExitConsole(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\exit.txt"));
                return;
            }
            Environment.Exit(0);
        }
    }
}
