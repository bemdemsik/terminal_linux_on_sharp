using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class Command
    {
        static public List<string> args = new List<string>();
        static public List<string> values = new List<string>();
        static public List<string> flag = new List<string>();
        static public void SplitCommand(string[] command)
        {
            flag.Clear();
            args.Clear();
            values.Clear();
            int i = 0;
            foreach(string _args in command)
            {
                i++;
                if (i == 1)
                    continue;
                if (_args.ToCharArray()[0] == '-')
                    args.Add(_args);
                else
                    values.Add(_args);
            }
        }
        static public bool CheckArguments()
        {
            foreach (string value in args)
            {
                if (!flag.Contains(value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
