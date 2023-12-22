using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    
    public static class Cat
    {
        static bool isN = false;
        static string inputUserText = string.Empty;
        static bool isB = false;
        static List<string> _arguments = new List<string>() { "-b", "-E", "-n", "->", "-h" };
        static List<string> _files = new List<string>();
        static List<string> _userArguments = new List<string>();
        public static void Catenate(string[] command)
        {
            inputUserText = string.Empty;

            if (command.Length == 1)
            {
                inputUserText = ReadText();
                Console.WriteLine(inputUserText);

                return;
            }

            isN = false;
            isB = false;
            _userArguments.Clear();
            _files.Clear();

            if (!CheckArguments(command[0], command))
            {
                return;
            }

            Console.WriteLine(ReadFile(_files, _userArguments));
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
                    try
                    {
                        if (value.ToCharArray()[1] == 'h')
                        {
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\cat.txt"));
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalid argument '-'");
                        return false;
                    }

                    if (_arguments.Contains(value))
                    {
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
                    _files.Add(value);
                }
            }

            return true;
        }

        public static string ReadText()
        {
            string userInput = string.Empty;

            while (true)
            {
                userInput += Console.ReadLine();

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.F1)
                {
                    break;
                }

                userInput += '\n';

            }

            return userInput;
        }

        public static List<string> SetArguments(List<string> arguments)
        {
            List<string> newArguments = new List<string>();

            foreach (var value in arguments)
            {
                if (value == "->")
                {
                    newArguments.Add(value);
                }
                if (value == "-b" && !isN)
                {
                    isB = true;
                    newArguments.Add(value);
                }

                if (value == "-n" && !isB)
                {
                    isN = true;
                    newArguments.Add(value);
                }

                if (value == "-E")
                {
                    newArguments.Add(value);
                }
            }

            return newArguments;
        }

        public static void WriterFile(string fileName, string text, string path, bool needCreate)
        {
            try
            {
                if (needCreate)
                {
                    using (File.Create(path + "\\" + fileName)) ;
                }

                using (StreamWriter writer = new StreamWriter(path + "\\" + fileName, true))
                {
                    writer.Write(text);

                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something when wrong");
                return;
            }
        }

        public static string CheckPath(string file)
        {
            string path = string.Empty;

            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + file))
            {
                path = Directory.GetCurrentDirectory() + "\\" + file;
                return path;
            }
            else if (File.Exists(file))
            {
                return file;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CheckDirectory(string path)
        {
            string _path = string.Empty;

            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + path))
            {
                _path = Directory.GetCurrentDirectory() + "\\" + path;
                return _path;
            }
            else if (Directory.Exists(path))
            {
                _path = path;
                return _path;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ReadFile(List<string> files, List<string> arguments)
        {
            string text = string.Empty;
            arguments = SetArguments(arguments);

            int countFiles = 0;

            if (arguments.Contains("->"))
            {
                if (arguments.Count > 1)
                {
                    return "error: -> does not accept arguments";
                }

                foreach (var value in files)
                {
                    if (value.Contains('/') || value.Contains('\\'))
                    {
                        bool isFile = true;
                        string file = CheckPath(value.Replace('/', '\\'));

                        if (file == string.Empty)
                        {
                            isFile = false;
                        }

                        string path = value.Replace('/', '\\');
                        string currentPath = string.Empty;
                        string[] pathFile = path.Split((new char[] { '\\' }), StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < pathFile.Length - 1; i++)
                        {
                            currentPath += pathFile[i] + "\\";
                        }

                        string checkName = pathFile[pathFile.Length - 1];
                        string[] checkExtension = checkName.Split('.');

                        if (checkExtension.Length != 2)
                        {
                            return "The file must contain the txt extension";
                        }
                        else if (checkExtension.Length == 2 && checkExtension[1] != "txt")
                        {
                            return "The file must contain the txt extension";
                        }

                        currentPath = CheckDirectory(currentPath);

                        if (currentPath == string.Empty && !isFile)
                        {
                            return "Path not found";
                        }

                        if (currentPath != string.Empty && !isFile)
                        {
                            if (countFiles == 0)
                            {
                                inputUserText = ReadText();
                            }

                            WriterFile(pathFile[pathFile.Length - 1], inputUserText, currentPath, true);
                        }

                        if (isFile)
                        {
                            if (countFiles == 0)
                            {
                                inputUserText = ReadText();
                            }

                            WriterFile(pathFile[pathFile.Length - 1], "\n" + inputUserText, currentPath, false);
                        }
                    }
                    else
                    {
                        string file = CheckPath(value.ToString());
                        string checkDirectory = CheckDirectory(Directory.GetCurrentDirectory() + "\\" + value.ToString());

                        string[] extension = value.Split('.');

                        if (extension.Length != 2)
                        {
                            return "The file must contain the txt extension";
                        }
                        else if (extension.Length == 2 && extension[1] != "txt")
                        {
                            return "The file must contain the txt extension";
                        }

                        if (checkDirectory != string.Empty)
                        {
                            return "The file name must be specified";
                        }

                        if (countFiles == 0)
                        {
                            inputUserText = ReadText();
                        }

                        if (file == string.Empty)
                        {
                            WriterFile(value.ToString(), inputUserText, Directory.GetCurrentDirectory(), true);
                        }
                        else
                        {
                            WriterFile(value.ToString(), "\n" + inputUserText, Directory.GetCurrentDirectory(), false);
                        }
                    }
                    countFiles++;
                }

                return "";
            }

            foreach (var value in files)
            {
                string pathFile = value;
                string nameFile = string.Empty;
                string[] path;
                int count = 0;
                int countStrings = 0;

                pathFile = CheckPath(value);

                if (pathFile == string.Empty)
                {
                    return "File not found";
                }

                try
                {
                    using (StreamReader reader = new StreamReader(pathFile))
                    {
                        if (countFiles != 0)
                        {
                            text += "\n" + "-------------------------" + "\n";
                        }
                        string read = string.Empty;
                        while ((read = reader.ReadLine()) != null)
                        {
                            int length = File.ReadAllLines(pathFile).Length;

                            if (count > 0 && count < length)
                            {
                                text += "\n";
                            }

                            countStrings++;

                            if (arguments.Contains("-b"))
                            {
                                if (read == string.Empty)
                                {
                                    countStrings--;
                                }
                                else
                                {
                                    read += " " + countStrings.ToString();
                                }
                            }

                            if (arguments.Contains("-n"))
                            {
                                read += " " + countStrings.ToString();
                            }

                            if (arguments.Contains("-E"))
                            {
                                read += "$";
                            }

                            text += read;
                            count++;
                        }

                        countFiles++;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("File " + nameFile + " not found");
                }
            }

            return text;
        }
    }
}
