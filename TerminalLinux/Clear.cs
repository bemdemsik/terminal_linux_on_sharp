using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class Clear
    {
        public static void ClearConsole(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\clear.txt"));
                return;
            }
            Console.Clear();
        }
    }
}
