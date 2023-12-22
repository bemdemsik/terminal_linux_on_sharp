using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace TerminalLinux
{
    public static class Processes
    {
        static bool isA = false;
        static bool isLowerA = false;
        static List<string> _arguments = new List<string>() { "-A", "-a", "-p", "-h" };
        static List<string> _inputs = new List<string>();
        static List<string> _userArguments = new List<string>();
        public static void ShowProcesses(string[] command)
        {
            isA = false;
            isLowerA = false;
            _inputs.Clear();
            _userArguments.Clear();

            if (!CheckArguments(command[0], command))
            {
                return;
            }

            string text = ReadProcesses(_userArguments, _inputs);

            if (text == string.Empty)
            {
                Console.WriteLine("Process(es) with the specified id were not found");
                return;
            }

            string[] array = text.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < array.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Console.Write(array[i]);
                }
                else
                {
                    int left = Console.CursorLeft;
                    Console.SetCursorPosition(70, Console.CursorTop);
                    Console.Write(array[i]);
                    Console.SetCursorPosition(left, Console.CursorTop);
                }

                if (i == array.Length - 1)
                {
                    Console.Write("\n");
                }
            }
        }

        public static bool CheckArguments(string command, string[] userInput)
        {
            foreach (var value in userInput)
            {
                if (value == command)
                {
                    continue;
                }

                if (value.ToCharArray()[0] == '-')
                {
                    if (_arguments.Contains(value))
                    {
                        if (value == "-h")
                        {
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\ps.txt"));
                            return false;
                        }

                        if (!_userArguments.Contains(value))
                        {
                            _userArguments.Add(value);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Invalid key " + value);
                        return false;
                    }
                }
                else
                {
                    _inputs.Add(value);
                }
            }

            return true;
        }

        public static List<string> SetArguments(List<string> arguments)
        {
            List<string> newArguments = new List<string>();

            foreach (var value in arguments)
            {
                if (value == "-A" && !isLowerA)
                {
                    isA = true;
                    newArguments.Add(value);
                }

                if (value == "-a" && !isA)
                {
                    isLowerA = true;
                    newArguments.Add(value);
                }

                if (value == "-p")
                {
                    newArguments.Add(value);
                }
            }

            return newArguments;
        }

        public static string ReadProcesses(List<string> arguments, List<string> inputs)
        {
            arguments = SetArguments(arguments);
            string text = string.Empty;

            try
            {
                Process[] processes;
                processes = Process.GetProcesses();

                if (arguments.Count == 0 || arguments.Contains("-A"))
                {
                    text = GetProcesses(processes, true);
                }

                if (arguments.Contains("-a"))
                {
                    text = GetProcesses(processes, false);
                }

                if (arguments.Contains("-p"))
                {
                    if (!TakeValues())
                    {
                        return string.Empty;
                    }

                    text = GetProcessesById(processes, isLowerA);
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
            }

            return text;
        }

        private static bool TakeValues()
        {
            if (_inputs.Count == 0)
            {
                Console.WriteLine("Incorrect use of the -p flag. Use the man reference for more information");
                return false;
            }

            foreach (var value in _inputs)
            {
                if (!value.All(char.IsDigit))
                {
                    Console.WriteLine("The ID must contain only numbers");
                    return false;
                }
            }

            return true;
        }

        private static string GetProcessesById(Process[] processes, bool isBackground)
        {
            string text = string.Empty;
            int countProcess = 0;

            if (isBackground)
            {
                foreach (var process in processes)
                {
                    if (process.MainWindowHandle != IntPtr.Zero)
                    {
                        if (_inputs.Contains(process.Id.ToString()))
                        {
                            if (countProcess > 0 && countProcess < processes.Length)
                            {
                                text += "\n";
                            }

                            text += "Process " + process.ProcessName + "\t ID " + process.Id + "\t";
                            countProcess++;
                        }
                    }
                }
            }
            else
            {
                foreach (var process in processes)
                {
                    if (_inputs.Contains(process.Id.ToString()))
                    {
                        if (countProcess > 0 && countProcess < processes.Length)
                        {
                            text += "\n";
                        }

                        text += "Process " + process.ProcessName + "\t ID " + process.Id + "\t";
                        countProcess++;
                    }
                }
            }

            return text;
        }

        private static string GetProcesses(Process[] processes, bool isBackground)
        {
            string text = string.Empty;
            int countProcess = 0;

            if (isBackground)
            {
                foreach (var process in processes)
                {
                    if (countProcess > 0 && countProcess < processes.Length)
                    {
                        text += "\n";
                    }

                    text += "Process " + process.ProcessName + "\t ID " + process.Id + "\t";
                    countProcess++;
                }
            }
            else
            {
                foreach (var process in processes)
                {
                    if (process.MainWindowHandle != IntPtr.Zero)
                    {
                        if (countProcess > 0 && countProcess < processes.Length)
                        {
                            text += "\n";
                        }

                        text += "Process " + process.ProcessName + "\t ID " + process.Id + "\t";
                    }
                    countProcess++;
                }
            }

            return text;
        }
    }
}
