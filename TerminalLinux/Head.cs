using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class Head
    {
        static List<string> commandArguments;
        public static void ReadLines(string command, string path)
        {
            List<string> commandSplit = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            commandArguments = new List<string>();
            List<string> values = new List<string>();
            bool check = false;
            int count = 10;
            Command.SplitCommand(commandSplit.ToArray());
            Command.flag.Add("-z");
            Command.flag.Add("-v");
            Command.flag.Add("-n");
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
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\head.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            foreach (string arg in commandSplit)
            {
                if (check == true)
                {
                    if (int.TryParse(arg, out int number))
                        count = number;
                    else
                    {
                        Console.WriteLine("invalid number of lines:" + number);
                        return;
                    }
                    check = false;
                    continue;
                }
                if (arg == "head")
                    continue;
                if (arg.ToCharArray()[0] == '-' && arg.ToCharArray()[1] == 'n')
                    check = true;
                if (arg.ToCharArray()[0] == '-')
                    commandArguments.Add(arg);
                else
                    values.Add(arg);
            }
            foreach (string value in values)
            {
                string _value = "";
                if (commandArguments.Contains("-v"))
                    _value = value;
                if (!File.Exists(path + "\\" + value))
                {
                    GetResultInConsole(ReadLines(value, count), commandArguments.Contains("-z"), _value);
                }
                else
                {
                    GetResultInConsole(ReadLines(path + "\\" + value, count), commandArguments.Contains("-z"), _value);
                }
            }
        }
        private static List<string> ReadLines(string path, int count)
        {
            count++;
            List<string> arrList = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int i = 0;
                    while (true)
                    {
                        arrList.Add(sr.ReadLine());
                        i++;
                        if (i == count-1)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("File not found");
                return null;
            }
            return arrList;
        }
        private static void GetResultInConsole(List<string> arrList, bool check, string fileName)
        {
            if (arrList == null)
            {
                return;
            }
            try
            {
                if (fileName != "")
                    Console.WriteLine("FileName:" + fileName);
                foreach (string str in arrList)
                {
                    Console.Write(str);
                    if (check)
                        Console.WriteLine('.');
                    else
                        Console.WriteLine();
                }
                Console.WriteLine("\n");
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка\nПроверьте данные");
            }
        }
    }
}
