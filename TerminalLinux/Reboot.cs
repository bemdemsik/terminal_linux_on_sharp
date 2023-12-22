using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class Reboot
    {
        public static void RebootPC(string[] command)
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
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\reboot.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            System.Diagnostics.Process.Start("shutdown", "/r /t 0");
        }
    }
}
