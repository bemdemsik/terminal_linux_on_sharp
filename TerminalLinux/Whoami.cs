using System;
using System.Threading;
using System.Reflection;
using System.IO;
namespace TerminalLinux
{
    static public class Whoami
    {
        static public void ShowWhoAmI(string[] arguments)
        {
            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-h");
            CommandLebedev.flagCommand.Add("-V");
            if (!CommandLebedev.Fill(arguments))
                return;
            if (CommandLebedev.values.Count > 0)
                Console.WriteLine("whoami: error syntax");
            else if (CommandLebedev.flagEnter.Contains("-V") || CommandLebedev.flagEnter.Contains("-h"))
            {
                if (CommandLebedev.flagEnter.Contains("-V"))
                    Console.WriteLine("whoami (GNU coreutils) 8.32");
                if (CommandLebedev.flagEnter.Contains("-h"))
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\whoami.txt"));
            }
            else if(arguments.Length == 1) 
                Console.WriteLine(Environment.UserName);
        }

}
}
