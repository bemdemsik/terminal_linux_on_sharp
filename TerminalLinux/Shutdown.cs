using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    class Shutdown
    {
        public static void ShutDown(string[] command)
        {
            if (command[0] == "reboot")
            {
                System.Diagnostics.Process.Start("shutdown", "/r /t 0");
                return;
            }
            string shutdown = "shutdown";
            string argument = "";
            Command.SplitCommand(command);
            Command.flag.Add("-s");
            Command.flag.Add("-t");
            Command.flag.Add("-f");
            Command.flag.Add("-r");
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
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\shutdown.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            int _time = 0;
            if (Command.args.Contains("-t"))
            {
                foreach(string value in Command.values)
                {
                    if (int.TryParse(value, out int time))
                        if (time < 0)
                            Console.WriteLine("Invalid number");
                        else
                        {
                            argument += "/t " + time + " ";
                            _time = time;
                        }
                }
            }
            if(_time==0)
                if(!shutdown.Contains("/t"))
                    argument += "/t " + 15 + " ";
            if (Command.args.Contains("-f"))
                argument += "/f ";
            if (Command.args.Contains("-r"))
            {
                argument += "/r ";
                Console.WriteLine("Reboot through:" + _time);
            }
            else
            {
                argument += "/s ";
                Console.WriteLine("Shudown through:" + _time);
            }

            System.Diagnostics.Process.Start(shutdown, argument);
        }
    }
}
