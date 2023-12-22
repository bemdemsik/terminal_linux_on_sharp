using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Reflection;

namespace TerminalLinux
{
    public static class ChangeDirectory
    {
        /// <summary>
        /// Egorov Daniil, 01.06.2023
        /// </summary>
        /// <param name="oldDirectory"></param>
        /// <param name="newDirectory"></param>
        /// <returns></returns>
        static List<string> _userArguments = new List<string>();
        static List<string> _values = new List<string>();
        static List<string> _allArguments = new List<string>() { "-e", "-h" };
        public static void Change(string[] command)
        {
            _userArguments.Clear();
            _values.Clear();

            string newPath = command[command.Length - 1];
            string[] fullPath = newPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string[] fullCommand = newPath.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (newPath == "cd" || fullPath[0] == "C:" && fullPath.Length == 1)
            {
                Directory.SetCurrentDirectory("C:\\");
                return;
            }

            if (fullPath[0] == "D:" && fullPath.Length == 1)
            {
                Directory.SetCurrentDirectory("D:\\");
                return;
            }

            if (fullCommand.Length > 2)
            {
                Console.WriteLine("cd: \t too many arguments");
                return;
            }

            string result = CheckRollback(newPath);

            if (result != string.Empty)
            {
                Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + "\\" + result);
                return;
            }

            if (!CheckArguments(command[0], command))
            {
                return;
            }

            if (File.Exists(fullPath[fullPath.Length - 1]))
            {
                Console.WriteLine("There should be a directory, not a file");
                return;
            }

            if (!Directory.Exists(newPath))
            {
                if (_userArguments.Contains("-e"))
                {
                    Console.WriteLine("Path not found");
                }

                return;
            }

            Directory.SetCurrentDirectory(newPath);
        }

        public static bool CheckArguments(string command, string[] userInput)
        {
            for (int i = 0; i < userInput.Length; i++)
            {
                if (command == userInput[i])
                {
                    continue;
                }

                if (userInput[i].ToCharArray()[0] == '-')
                {
                    if (_allArguments.Contains(userInput[i]))
                    {
                        try
                        {
                            if (userInput[i] == "-h")
                            {
                                Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\cd.txt"));
                                return false;
                            }

                            if (userInput[i] == "-e")
                            {
                                _userArguments.Add("-e");
                                return true;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("File or path not found");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid argument " + userInput[i]);
                        return false;
                    }

                }
            }

            return true;
        }

        public static string CheckRollback(string userInput)
        {
            string result = string.Empty;
            string[] textRollback = userInput.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (userInput.Contains("../"))
            {
                foreach (var value in textRollback)
                {
                    if (value == "../")
                    {
                        result += "../";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            return result;
        }
    }
}
