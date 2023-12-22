using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    public static class CommandLebedev
    {
        static public List<string> flagCommand = new List<string>();
        static public List<string> flagEnter = new List<string>();
        static public List<string> values = new List<string>();
        public static bool Fill(string[] arguments)
        {
            flagEnter = new List<string>();
            values = new List<string>();
            for (int i = 1; i < arguments.Length; i++)
                if (arguments[i].ToCharArray()[0] == '-')
                {
                    if (!flagCommand.Contains(arguments[i]))
                    {
                        Console.WriteLine("Invalid key " + arguments[i]);
                        return false;
                    }
                    else flagEnter.Add(arguments[i]);
                }
                else values.Add(arguments[i]);
            return true;
        }
    }
}
