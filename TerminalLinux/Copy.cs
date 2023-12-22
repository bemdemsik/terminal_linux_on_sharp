using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{

    public static class Copy
    {
        static bool isF = false;
        static bool isI = false;
        static string[] checkFolders = new string[0];
        static List<string> _arguments = new List<string>() { "-f", "-i", "-r", "-h" };
        static List<string> _files = new List<string>();
        static List<string> _userArguments = new List<string>();

        public static void Copying(string[] command)
        {
            if (command.Length == 1)
            {
                Console.WriteLine("The cp command accepts values. For more information, use man");
                return;
            }

            checkFolders = new string[0];
            isF = false;
            isI = false;
            _userArguments.Clear();
            _files.Clear();

            if (!CheckArguments(command[0], command))
            {
                return;
            }

            if (_files.Count > 1)
            {
                if (CheckIdenticalPath(_files[0], _files[1]))
                {
                    return;
                }
            }

            string result = string.Empty;

            if (_userArguments.Contains("-r"))
            {
                if (_files.Count > 2)
                {
                    Console.WriteLine("cp -r: too many arguments");
                    return;
                }

                result = CopyingDirectories(_files, _userArguments);
            }
            else
            {
                result = CopyingFiles(_files, _userArguments);
            }


            Console.Write(result + "\n");

        }

        public static bool CheckIdenticalPath(string sourcePath, string destinationPath)
        {
            string[] sourceFullPath = sourcePath.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string[] destinationFullPath = destinationPath.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            string checkSourcePath = string.Join("", sourceFullPath);
            string checkDestinationPath = string.Join("", destinationFullPath);

            if (checkSourcePath == checkDestinationPath)
            {
                Console.WriteLine("The paths are identical");
                return true;
            }

            return false;

        }

        public static string CopyingDirectories(List<string> files, List<string> arguments)
        {
            string destinationFile = string.Empty;
            string sourceFile = string.Empty;
            string sourcePath = string.Empty;
            bool isCurrentDirectory = false;
            string destinationPath = string.Empty;

            sourcePath = CheckDirectory(files[0], false);

            if (sourcePath == string.Empty)
            {
                return "Path not found";
            }

            if (files.Count == 1)
            {
                isCurrentDirectory = true;
            }
            else
            {
                destinationPath = CheckDirectory(files[1], true);
                destinationFile = CheckPath(files[1]);
            }

            sourceFile = CheckPath(files[0]);

            arguments = SetArguments(arguments);

            if (sourceFile != string.Empty || destinationFile != string.Empty)
            {
                return "The -r parameter accepts directories, not files";
            }

            string[] path = null;
            string destinationDirectory = string.Empty;
            string nameDirectory = string.Empty;

            if (isCurrentDirectory)
            {
                destinationDirectory = Directory.GetCurrentDirectory();
            }
            else
            {
                string newDestination = string.Empty;

                path = destinationPath.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < path.Length - 1; i++)
                {
                    newDestination += path[i] + "\\";
                }

                destinationDirectory = CheckDirectory(newDestination, false);

                if (destinationDirectory == string.Empty)
                {
                    return "Directory or path not found";
                }

                nameDirectory = path[path.Length - 1];
            }


            if (arguments.Contains("-i"))
            {
                CopyDirectories(sourcePath, destinationDirectory + nameDirectory, true, arguments);
            }

            if (arguments.Contains("-f") || arguments.Count == 1)
            {
                CopyDirectories(sourcePath, destinationDirectory + nameDirectory, false, arguments);
            }

            return string.Empty;
        }

        public static string CopyingFiles(List<string> files, List<string> arguments)
        {
            string checkPath = string.Empty;
            string destinationFile = string.Empty;
            string sourceFile = string.Empty;
            string nameFile = string.Empty;
            string nameDestinationFile = string.Empty;
            string destinationPath = string.Empty;
            string sourcePath = files[0].Replace('/', '\\');
            string[] destinationFullPath;
            string[] sourceFullPath = sourcePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string[] checkExpansion = sourceFullPath[sourceFullPath.Length - 1].Split('.');

            sourcePath = CheckPath(sourcePath);

            if (sourcePath == string.Empty)
            {
                return "File or path not found";
            }

            if (checkExpansion.Length == 1)
            {
                return "The file must contain the extension";
            }

            string expansionFile = checkExpansion[1];

            nameFile = sourceFullPath[sourceFullPath.Length - 1];


            if (files.Count > 1)
            {
                destinationPath = files[1].Replace('/', '\\');
                destinationFullPath = destinationPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                nameDestinationFile = destinationFullPath[destinationFullPath.Length - 1];
                string[] nameThisFile = nameDestinationFile.Split('.');

                if (nameThisFile.Length == 2)
                {
                    destinationPath = destinationPath.Replace(nameDestinationFile, "");
                    string checkDirectory = CheckDirectory(destinationPath, false);

                    if (checkDirectory == string.Empty)
                    {
                        return "Directory not found";
                    }

                    if (nameThisFile[1] == expansionFile)
                    {
                        destinationPath += "\\" + nameDestinationFile;
                    }
                    else
                    {
                        return "The file extensions must be identical";
                    }
                }
                else if (nameThisFile.Length == 1)
                {
                    string checkDirectory = CheckDirectory(destinationPath, false);

                    if (checkDirectory == string.Empty)
                    {
                        return "Directory or extensions not found";
                    }

                    destinationPath += "\\" + nameFile;
                }
            }
            else
            {
                destinationPath = Directory.GetCurrentDirectory() + "\\" + nameFile;
                nameDestinationFile = nameFile;
            }

            arguments = SetArguments(arguments);

            try
            {
                if (arguments.Count == 0)
                {
                    if (!File.Exists(destinationPath))
                    {
                        File.Copy(sourcePath, destinationPath);
                    }
                    else
                    {
                        return "The file " + destinationPath + " already exists";
                    }

                    return "";
                }

                if (arguments.Contains("-f"))
                {
                    if (File.Exists(destinationPath))
                        File.Delete(destinationPath);
                }

                if (arguments.Contains("-i"))
                {
                    if (File.Exists(destinationPath))
                    {
                        if (!InteractiveInput(destinationPath))
                        {
                            return string.Empty;
                        }

                        File.Delete(destinationPath);
                    }
                }

                File.Copy(sourcePath, destinationPath);
            }
            catch (IOException)
            {
                return "Something went wrong";
            }

            return "";
        }

        public static void CopyDirectories(string fromDirectory, string toDirectory, bool isInteractive, List<string> arguments)
        {
            bool containArguments = arguments.Count > 1 ? true : false;
            bool isY = false;
            string[] folders = Directory.GetDirectories(fromDirectory);

            if (checkFolders.Length == 0)
            {
                Array.Resize(ref checkFolders, checkFolders.Length + 1);
                checkFolders[checkFolders.Length - 1] = fromDirectory;
            }

            Directory.CreateDirectory(toDirectory);

            foreach (string file in Directory.GetFiles(fromDirectory))
            {
                string[] fileName = file.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (File.Exists(toDirectory + "\\" + fileName[fileName.Length - 1]))
                {
                    if (arguments.Count > 1)
                    {
                        containArguments = true;
                        if (isInteractive)
                        {
                            if (InteractiveInput(toDirectory + "\\" + fileName[fileName.Length - 1]))
                            {
                                File.Delete(toDirectory + "\\" + fileName[fileName.Length - 1]);
                                isY = true;
                            }
                            else
                            {
                                isY = false;
                            }
                        }
                        else
                        {
                            File.Delete(toDirectory + "\\" + fileName[fileName.Length - 1]);
                        }
                    }
                }
                else
                {
                    isY = true;
                }

                string[] path = Path.GetFileName(file).Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                string fileInFolder = toDirectory + "\\" + path[path.Length - 1];

                if (!containArguments)
                {
                    if (!File.Exists(toDirectory + "\\" + fileName[fileName.Length - 1]))
                    {
                        File.Copy(file, fileInFolder);
                    }
                    else
                    {
                        Console.WriteLine("The file " + toDirectory + "\\" + fileName[fileName.Length - 1] + " already exists");
                    }
                }

                else if (!isInteractive || isY)
                {
                    try
                    {
                        File.Copy(file, fileInFolder);
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            int checkCount = 0;

            foreach (var value in folders)
            {
                for (int i = 0; i < checkFolders.Length; i++)
                {
                    if (checkFolders[i] == value)
                    {
                        checkCount++;
                    }
                }

                if (checkCount == 0)
                {
                    string[] path = Path.GetFileName(value).Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    CopyDirectories(value, toDirectory + "\\" + path[path.Length - 1], isInteractive, arguments);
                    checkFolders[checkFolders.Length - 1] = toDirectory + "\\" + path[path.Length - 1];
                }

                checkCount = 0;

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
                path = file;
                return path;
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool InteractiveInput(string path)
        {
            string[] file = path.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("The file " + string.Join("\\", file) + " has already been created. Do you want to overwrite the file?");
            Console.WriteLine("Y/N ");

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (key == ConsoleKey.N)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        public static List<string> SetArguments(List<string> arguments)
        {
            List<string> newArguments = new List<string>();

            foreach (var value in arguments)
            {
                if (value == "-f" && !isI)
                {
                    isF = true;
                    newArguments.Add(value);
                }

                if (value == "-i" && !isF)
                {
                    isI = true;
                    newArguments.Add(value);
                }

                if (value == "-r")
                {
                    newArguments.Add(value);
                }
            }

            return newArguments;
        }

        public static string CheckDirectory(string path, bool isDestination)
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
                if (isDestination)
                {
                    Directory.CreateDirectory(path);
                    _path = path;
                    return _path;
                }
                else
                {
                    return string.Empty;
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
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\cp.txt"));
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
                    _files.Add(value);
                }
            }

            if (_files.Count > 2)
            {
                Console.WriteLine("cp: too many arguments");
                return false;
            }
            else if (_files.Count < 1)
            {
                Console.WriteLine("cp: the command takes values");
                return false;
            }

            return true;
        }
    }
}
