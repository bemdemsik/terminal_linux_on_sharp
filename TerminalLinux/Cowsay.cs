using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class Cowsay
    {
        static readonly private string cow = @"
        \   ^__^
         \  (oo)\_______
            (__)\       )\/\
                ||-----||
                ||     ||";
        static readonly private string cowP = @"
        \   ^__^
         \  (@@)\_______
            (__)\       )\/\
                ||-----||
                ||     ||";
        static readonly private string cowG = @"
        \   ^__^
         \  ($$)\_______
            (__)\       )\/\
                ||-----||
                ||     ||";
        static readonly private string robot = @"
                  _____
          \      /     \
           \    | () () |
                 \  ^  /
                  |||||
            ^^^^^^^^^^^^^^^^^^
            / ****************\
          /  ******************  \
         |   **XXX**XXXX**XXX**   |
         |   **XXX**XXXX**XXX**   |
          \  *****************  /
            \ ****************/
             ^^^^^^^^^^^^^^^^^^
         ";

        static public void CowSay(string[] command)
        {
            Command.SplitCommand(command);
            Command.flag.Add("-r");
            Command.flag.Add("-p");
            Command.flag.Add("-g");
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
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\cowsay.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            string message = "";
            for (int i = 0; i < Command.values.Count(); i++)
            {
                message += Command.values[i] + " ";
            }
            message = message.Replace('\n', ' ');
            if (message.Length <= 50)
            {
                Console.WriteLine(" " + new string('_', message.Length + 2));
                Console.WriteLine(string.Format("< {0} >", message));
                Console.WriteLine(" " + new string('-', message.Length + 2));
            }
            else
            {
                int countString = 0;
                string _message = "";
                Console.WriteLine(" " + new string('_', 50 + 2)); 
                string[] arrayMessage = message.Split(' ');
                for(int i =0; i<arrayMessage.Length;i++)
                {
                    if((_message).Length>=50)
                    {
                        _message = _message.TrimEnd(' ');
                        while(_message.Length>=50)
                        {
                            _message = _message.Remove(_message.LastIndexOf(' '));
                            i--;
                        }
                        if (countString == 0)
                        {
                            Console.Write("/ " + _message + @" ");
                            Console.SetCursorPosition(53, Console.CursorTop);
                            Console.WriteLine(@" \");
                            countString++;
                            _message = "";
                            continue;
                        }
                        if (countString>0)
                        {
                            Console.Write("|" + _message + @" ");
                            Console.SetCursorPosition(53, Console.CursorTop);
                            Console.WriteLine(@" |");
                            countString++;
                            _message = "";
                            continue;
                        }
                    }
                    _message += arrayMessage[i]+ " ";
                    if(arrayMessage.Length-1==i)
                    {
                        Console.Write(@"\ " + _message + @" ");
                        Console.SetCursorPosition(53, Console.CursorTop);
                        Console.WriteLine(@" /");
                    }
                }
                Console.WriteLine(" " + new string('-', 50 + 2));
            }
            if (Command.args.Contains("-r"))
                Console.WriteLine(robot);
            else if (Command.args.Contains("-g"))
                Console.WriteLine(cowG);
            else if (Command.args.Contains("-p"))
                Console.WriteLine(cowP);
            else
                Console.WriteLine(cow);
        }
    }
}
