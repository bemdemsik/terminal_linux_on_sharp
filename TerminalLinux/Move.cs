using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    public static class Move
    {
        static string[] checkFolders = new string[0];
        static bool isN = false;
        static bool isF = false;
        static bool isI = false;
        static List<string> _arguments = new List<string>() { "-f", "-i", "-n", "-h" };
        static List<string> _files = new List<string>();
        static List<string> _userArguments = new List<string>();
        public static void Moving(string[] commands)
        {
            if (commands.Length == 1)
            {
                Console.WriteLine("move: command accepts arguments and values");
                return;
            }

            checkFolders = new string[0];
            isN = false;
            isF = false;
            isI = false;
            _userArguments.Clear();
            _files.Clear();

            if (!CheckArguments(commands[0], commands))
            {
                return;
            }

            int count = 0;

            foreach (var value in _files)
            {
                if (count == _files.Count - 1)
                {
                    break;
                }

                if (CheckIdenticalPath(value, _files[_files.Count - 1]))
                {
                    return;
                }

                count++;
            }

            string result = CheckEvent(_files, _userArguments);

            Console.WriteLine(result);
        }

        public static List<string> SetArguments(List<string> arguments)
        {
            List<string> newArguments = new List<string>();

            foreach (var value in arguments)
            {
                if (value == "-f" && !isI && !isN)
                {
                    isF = true;
                    newArguments.Add(value);
                }

                if (value == "-i" && !isF && !isN)
                {
                    isI = true;
                    newArguments.Add(value);
                }

                if (value == "-n" && !isI && !isF)
                {
                    isN = true;
                    newArguments.Add(value);
                }
            }

            return newArguments;
        }

        public static bool CheckIdenticalPath(string sourcePath, string destinationPath)
        {
            string currentPath = string.Empty;

            if (sourcePath.Contains("*"))
            {
                string[] fullPath = sourcePath.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < fullPath.Length - 1; i++)
                {
                    currentPath += fullPath[i] + "\\";
                }
            }
            else
            {
                currentPath = sourcePath;
            }

            string[] sourceFullPath = currentPath.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
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

        public static string CopyFiles(string sourceFile, string destinationPath, List<string> arguments, string allFiles)
        {
            string fileName = string.Empty;
            bool isCopy = false;
            if (allFiles != string.Empty)
            {
                string[] files = Directory.GetFiles(sourceFile, allFiles);

                foreach (var value in Directory.GetFiles(sourceFile, allFiles))
                {
                    fileName = value.Replace('/', '\\').Split('\\')[value.Replace('/', '\\').Split('\\').Length - 1];

                    if (arguments.Contains("-i"))
                    {
                        if (File.Exists(destinationPath + "\\" + fileName))
                        {
                            if (InteractiveInput(destinationPath + "\\" + fileName))
                            {
                                File.Delete(destinationPath + "\\" + fileName);
                                isCopy = true;
                            }
                            else
                            {
                                isCopy = false;
                            }
                        }
                        else
                        {
                            isCopy = true;
                        }
                    }

                    if (arguments.Contains("-f"))
                    {
                        if (File.Exists(destinationPath + "\\" + fileName))
                        {
                            File.Delete(destinationPath + "\\" + fileName);
                        }

                        isCopy = true;
                    }

                    if (arguments.Contains("-n") || arguments.Count == 0)
                    {
                        if (File.Exists(destinationPath + "\\" + fileName))
                        {
                            Console.WriteLine("The file " + destinationPath + "\\" + fileName + " already exists");
                            isCopy = false;
                        }
                        else
                        {
                            isCopy = true;
                        }
                    }

                    try
                    {
                        if (isCopy)
                        {
                            File.Copy(sourceFile + "\\" + fileName, destinationPath.Replace('/', '\\') + "\\" + fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        return "Something went wrong";
                    }
                }

                return "";
            }
            else
            {
                fileName = sourceFile.Replace('/', '\\').Split('\\')[sourceFile.Replace('/', '\\').Split('\\').Length - 1];

                if (arguments.Contains("-i"))
                {
                    if (File.Exists(destinationPath + "\\" + fileName))
                    {
                        if (InteractiveInput(destinationPath + "\\" + fileName))
                        {
                            File.Delete(destinationPath + "\\" + fileName);
                            isCopy = true;
                        }
                        else
                        {
                            isCopy = false;
                        }
                    }
                    else
                    {
                        isCopy = true;
                    }
                }

                if (arguments.Contains("-f"))
                {
                    if (File.Exists(destinationPath + "\\" + fileName))
                    {
                        File.Delete(destinationPath + "\\" + fileName);
                    }

                    isCopy = true;
                }

                if (arguments.Contains("-n") || arguments.Count == 0)
                {
                    if (File.Exists(destinationPath + "\\" + fileName))
                    {
                        Console.WriteLine("The file " + destinationPath + "\\" + fileName + " already exists");
                        isCopy = false;
                    }
                    else
                    {
                        isCopy = true;
                    }
                }

                if (isCopy)
                {
                    try
                    {
                        File.Copy(sourceFile, destinationPath.Replace('/', '\\') + "\\" + fileName);
                    }
                    catch (Exception ex)
                    {
                        return "Something went wrong";
                    }
                }

                return "";
            }

        }

        public static string MoveDirectoriesAndFiles(List<string> files, List<string> arguments)
        {
            string destinationDirectory = string.Empty;
            destinationDirectory = CheckDirectory(files[files.Count - 1]).Replace("/", "\\");
            int count = 0;
            string[] path = null;

            foreach (var value in files)
            {
                string directory = string.Empty;
                string file = string.Empty;
                bool isDirectory = false;
                bool isFile = false;
                string expansion = string.Empty;
                string allFiles = string.Empty;

                if (count == files.Count - 1)
                {
                    break;
                }

                path = value.Replace("/", "\\").Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (path[path.Length - 1].Contains("*"))
                {
                    if (value.Contains("/") || value.Contains("\\"))
                    {
                        for (int i = 0; i < path.Length - 1; i++)
                        {
                            directory += path[i] + "\\";
                        }

                        directory = CheckDirectory(directory);

                        if (directory == string.Empty)
                        {
                            return "Directory not found";
                        }
                    }
                    else
                    {
                        directory = Directory.GetCurrentDirectory() + "\\";
                    }

                    string[] checkExpansion = path[path.Length - 1].Split('.');

                    if (checkExpansion.Length > 1)
                    {
                        expansion = checkExpansion[checkExpansion.Length - 1];
                        allFiles = "*." + expansion;
                    }
                    else
                    {
                        if (checkExpansion[0] != "*")
                        {
                            return "mv: error syntax";
                        }
                        else
                        {
                            allFiles = "*";
                        }

                    }

                    isFile = true;
                    isDirectory = false;
                }
                else
                {
                    allFiles = "";
                    file = CheckPath(value);

                    if (file == string.Empty)
                    {
                        isFile = false;

                        directory = CheckDirectory(value);

                        if (directory == string.Empty)
                        {
                            isDirectory = false;
                            return "Path not found";
                        }
                        else
                        {
                            isDirectory = true;
                        }
                    }
                    else
                    {
                        isFile = true;
                        isDirectory = false;
                    }
                }

                if (isFile)
                {
                    if (allFiles == "*")
                    {
                        CopyDirectories(directory, files[files.Count - 1], isI, arguments);
                    }
                    else if (allFiles.Contains("*."))
                    {
                        CopyFiles(directory, files[files.Count - 1], arguments, allFiles);
                    }
                    else if (allFiles.Length == 0)
                    {
                        CopyFiles(file, files[files.Count - 1], arguments, allFiles);
                    }
                }

                if (isDirectory)
                {
                    CopyDirectories(directory, files[files.Count - 1], isI, arguments);
                }

                count++;
            }

            return "";
        }

        public static void CopyDirectories(string fromDirectory, string toDirectory, bool isInteractive, List<string> arguments)
        {
            bool containArguments = false;
            bool isY = false;
            string[] folders = Directory.GetDirectories(fromDirectory);

            if (checkFolders.Length == 0)
            {
                Array.Resize(ref checkFolders, checkFolders.Length + 1);
                checkFolders[checkFolders.Length - 1] = fromDirectory;
            }

            if (!Directory.Exists(toDirectory))
            {
                Directory.CreateDirectory(toDirectory);
            }

            if (arguments.Count == 1 && arguments.Contains("-n") || arguments.Count == 0)
            {
                containArguments = false;
            }
            else
            {
                containArguments = true;
            }

            foreach (string file in Directory.GetFiles(fromDirectory))
            {
                string[] fileName = file.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (File.Exists(toDirectory + "\\" + fileName[fileName.Length - 1]))
                {
                    if (containArguments)
                    {
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
                        Console.WriteLine("The file " + toDirectory + "\\" + fileName[fileName.Length - 1] + " has already exists");
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

        public static string CheckEvent(List<string> files, List<string> arguments)
        {
            arguments = SetArguments(arguments);
            bool isDirectory = false;
            string path = files[files.Count - 1];
            string directory = string.Empty;
            bool isFile = false;
            string file = string.Empty;

            file = CheckPath(files[files.Count - 1]);
            directory = CheckDirectory(files[files.Count - 1]);

            if (file == string.Empty)
            {
                isFile = false;
            }
            else
            {
                if (!isF && !isI)
                {
                    return "File " + file + " is already exists";
                }

                isFile = true;
            }

            string destinationFile = files[files.Count - 1].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)[files[files.Count - 1].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Length - 1];
            string[] expansionDestinationFile = destinationFile.Split('.');

            if (expansionDestinationFile.Length >= 2)
            {
                isFile = true;
            }

            if (directory == string.Empty)
            {
                isDirectory = false;

            }
            else
            {
                isDirectory = true;
            }

            if (!isDirectory && !isFile)
            {
                return "Destination directory must exist and the files must have extensions";
            }

            if (isDirectory)
            {
                return MoveDirectoriesAndFiles(files, arguments);
            }

            if (isFile)
            {
                return RenameFile(files, arguments);
            }

            return string.Empty;
        }

        public static string RenameFile(List<string> files, List<string> arguments)
        {
            if (files.Count > 2)
            {
                return "mv: too many arguments to rename a file";
            }

            if (!File.Exists(files[0]))
            {
                return "mv: the file " + files[0] + " does not exist";
            }

            if (files[files.Count - 1].Contains('/') || files[files.Count - 1].Contains('\\'))
            {
                return "mv: incorrect new file name";
            }



            string sourcePath = string.Empty;
            string[] path = files[0].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < path.Length - 1; i++)
            {
                sourcePath += path[i] + "\\";
            }

            string sourceFile = files[0].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)[files[0].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Length - 1];
            string expansionSourceFile = sourceFile.Split('.')[sourceFile.Split('.').Length - 1];

            string destinationFile = files[1].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)[files[1].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Length - 1];
            string expansionDestinationFile = destinationFile.Split('.')[destinationFile.Split('.').Length - 1];

            if (expansionSourceFile != expansionDestinationFile)
            {
                return "The file extensions must be identical";
            }

            if (arguments.Contains("-i"))
            {
                if (File.Exists(sourcePath + "\\" + destinationFile))
                {
                    if (!InteractiveInput(sourcePath + "\\" + destinationFile))
                    {
                        return "";
                    }

                    File.Delete(sourcePath + "\\" + destinationFile);
                }
            }

            if (arguments.Contains("-f"))
            {
                if (File.Exists(sourcePath + "\\" + destinationFile))
                {
                    File.Delete(sourcePath + "\\" + destinationFile);
                }
            }

            if (arguments.Contains("-n") || arguments.Count == 0)
            {
                if (File.Exists(sourcePath + "\\" + destinationFile))
                {
                    return "The file already exists";
                }
            }

            File.Move(files[0], sourcePath + "\\" + destinationFile);

            return string.Empty;
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
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\mv.txt"));
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

            if (_files.Count < 2)
            {
                Console.WriteLine("The command takes at least 2 and more values");
                return false;
            }

            return true;
        }
    }
}
