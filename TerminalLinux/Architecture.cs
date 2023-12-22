using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class Architecture
    {
        static public void ExecuteArchitectue(string[] command)
        {
            Command.SplitCommand(command);
            Command.flag.Add("-h");
            Command.flag.Add("--help");
            if (!Command.CheckArguments())
            {
                Console.WriteLine("invalid flag");
                return;
            }
            if (Command.args.Contains("--help") || Command.args.Contains("-h"))
            {
                if (Command.args.Count == 1)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\arch.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            Console.WriteLine(Environment.Is64BitOperatingSystem ? "x86_64" : "x86");
        }
    }
}
