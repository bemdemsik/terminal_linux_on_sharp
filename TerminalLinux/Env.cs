using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class Env
    {
        public static void PrintValiables(string[] args)
        {
            try
            {
                if (args.Contains("-h") || args.Contains("--help"))
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\env.txt"));
                    return;
                }
                if (args.Length == 1)
                {
                    EnviromentVariables.GetVariables();
                }
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Contains("histsize"))
                        {
                            int size = Convert.ToInt32(args[i + 1]);
                            EnviromentVariables.HistSize = size;
                        }
                    }
                }
            }
            catch(Exception)
            {
                Console.WriteLine("An error occurred while executing the command. Check command syntax");
            }
        }
    }
}
