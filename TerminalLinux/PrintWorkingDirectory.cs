using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    public static class PrintWorkingDirectory
    {
        static List<string> _userArguments = new List<string>();
        static List<string> arguments = new List<string>() { "-h" };
        public static void Print(string[] commands)
        {
            if (commands.Length == 1)
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                return;
            }

            if (!CheckArguments(commands[0], commands))
            {
                return;
            }
            
        }

        public static bool CheckArguments(string command, string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == command)
                {
                    continue;
                }

                if (arguments.Contains(input[i]))
                {
                    try
                    {
                        if (input[i].ToCharArray()[1] == 'h')
                        {
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\pwd.txt"));
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid argument '-'");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
