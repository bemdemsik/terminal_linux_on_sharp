using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class List
    {
        static List<string> commandArguments;
        static public void ExecuteList(string command, string path)
        {
            List<string> commandSplit = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            commandArguments = new List<string>();
            List<string> values = new List<string>();
            Command.SplitCommand(commandSplit.ToArray());
            Command.flag.Add("-d");
            Command.flag.Add("-m");
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
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\ls.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            foreach (string arg in commandSplit)
            {
                if (arg == "ls")
                    continue;
                if (arg.ToCharArray()[0] == '-')
                    commandArguments.Add(arg);
                else
                    values.Add(arg);
            }
            if (values.Count == 0)
            {
                LoadList(path, path);
            }
            else
            {
                foreach (string value in values)
                {
                    LoadList(value, path);
                }
            }

        }
        static void LoadList(string value, string path)
        {
            List<string> result = new List<string>();
            try
            {
                if (Path.IsPathRooted(value))
                {
                    result.AddRange(Directory.GetFileSystemEntries(value));
                    CheckArguments(result);
                }
                else
                {
                    result.AddRange(Directory.GetFileSystemEntries(path + "\\" + value));
                    CheckArguments(result);
                }
            }
            catch
            {
                Console.WriteLine("cd:\t" + value + ": No such file or directory");
            }
        }
        static private void CheckArguments(List<string> basicList)
        {
            List<string> read = new List<string>();
            for (int i = 0; i < basicList.Count; i++)
            {
                if (Directory.Exists(basicList[i]))
                    basicList[i] += '/';
            }
            if (commandArguments.Contains("-d"))
            {
                for (int i = 0; i < basicList.Count; i++)
                {
                    if (Directory.Exists(basicList[i]))
                    {
                        read.Add(basicList[i]);
                    }
                }
            }
            else
            {
                read = basicList;
            }
            if(read.Count==0)
            {
                Console.WriteLine("No such directory");
            }
            read = read.Select(x => x.Split('\\')[x.Split('\\').Length - 1]).ToList();
            if (commandArguments.Contains("-r"))
            {
                read.Reverse();
            }
            foreach (string str in read)
            {
                if (commandArguments.Contains("-m"))
                {
                    Console.WriteLine(string.Join(",\t", read));
                    break;
                }
                Console.WriteLine("\t" + str);
            }
        }
    }
}
