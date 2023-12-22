using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace TerminalLinux
{
    public static class Unzip
    {
        static List<string> _arguments = new List<string>() { "-n", "-l", "-j", "-h" };
        static List<string> _files = new List<string>();
        static List<string> _userArguments = new List<string>();

        public static void Uncompress(string[] commands)
        {
            if (commands.Length == 1)
            {
                Console.WriteLine("unzip: the command takes values");
                return;
            }

            _files.Clear();
            _userArguments.Clear();

            if (!CheckArguments(commands[0], commands))
            {
                return;
            }


            string result = CheckEvent(_files, _userArguments);

            Console.WriteLine(result);

        }

        public static string CheckEvent(List<string> files, List<string> arguments)
        {
            string file = string.Empty;
            string directory = string.Empty;
            bool isDirectory = false;
            string folderName = string.Empty;

            file = CheckPath(files[0]);

            if (file == string.Empty)
            {
                return "unzip: the archive must exist";
            }

            string expansionFile = string.Empty;

            string nameArchive = string.Empty;
            string[] fullPath = files[0].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string fileWithExpansion = fullPath[fullPath.Length - 1];
            string expansion = string.Empty;

            string[] checkExpansion = fileWithExpansion.Split('.');

            if (checkExpansion.Length >= 2)
            {
                expansion = checkExpansion[checkExpansion.Length - 1];
            }

            if (expansion != "zip")
            {
                return "unzip: the name must contain the .zip extension";
            }

            for (int i = 0; i < checkExpansion.Length - 1; i++)
            {
                nameArchive += checkExpansion[i] + "\\";
            }

            string archive = string.Join("\\", fullPath);

            directory = CheckDirectory(files[files.Count - 1]);

            if (directory == string.Empty)
            {
                isDirectory = false;
                directory = Directory.GetCurrentDirectory();
            }
            else
            {
                isDirectory = true;
            }


            if (!Directory.Exists(directory + "\\" + nameArchive))
            {
                Directory.CreateDirectory(directory + "\\" + nameArchive);
            }


            if (files.Count == 1 || files.Count == 2 && isDirectory)
            {
                UncompressArchive(archive, directory + "\\" + nameArchive, arguments);

                return string.Empty;
            }

            int count = 0;

            foreach (var value in files)
            {
                bool isFile = false;

                if (count == 0)
                {
                    count++;
                    continue;
                }

                if (isDirectory)
                {
                    if (count == files.Count - 1)
                    {
                        break;
                    }
                }

                string[] fullValue = value.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                string[] checkExtension = fullValue[fullValue.Length - 1].Split('.');

                if (checkExtension.Length < 2)
                {
                    isFile = false;
                }
                else
                {
                    isFile = true;
                }

                if (isFile)
                {
                    UncompressFiles(archive, directory + "\\" + nameArchive, value, arguments);
                }
                else
                {
                    UncompressDirectories(archive, directory + "\\" + nameArchive, value, arguments);
                }

                count++;
            }


            return string.Empty;

        }

        public static void UncompressArchive(string archive, string path, List<string> arguments)
        {
            bool isJ = arguments.Contains("-j") ? true : false;
            bool isN = arguments.Contains("-n") ? true : false;

            if (isJ || isN)
            {
                using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
                {
                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                    {
                        string[] fullEntry = entry.ToString().Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        string newEntry = string.Join("", fullEntry);
                        string[] fileWithExtansion = fullEntry[fullEntry.Length - 1].Split('.');
                        string pathToFile = string.Empty;

                        if (fileWithExtansion.Length >= 2)
                        {
                            if (isJ)
                            {
                                if (isN)
                                {
                                    if (File.Exists(path + "\\" + fullEntry[fullEntry.Length - 1]))
                                    {
                                        Console.WriteLine("File " + path + "\\" + fullEntry[fullEntry.Length - 1] + " already exists");
                                        return;
                                    }
                                }

                                if (File.Exists(path + "\\" + fullEntry[fullEntry.Length - 1]))
                                {
                                    File.Delete(path + "\\" + fullEntry[fullEntry.Length - 1]);
                                }

                                entry.ExtractToFile(path + "\\" + fullEntry[fullEntry.Length - 1]);
                            }
                            else
                            {
                                for (int i = 0; i < fullEntry.Length - 1; i++)
                                {
                                    pathToFile += fullEntry[i] + "\\";
                                }

                                if (isN)
                                {
                                    if (File.Exists(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]))
                                    {
                                        Console.WriteLine("File " + path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1] + " already exists");
                                        return;
                                    }
                                }

                                if (!Directory.Exists(path + "\\" + pathToFile))
                                {
                                    Directory.CreateDirectory(path + "\\" + pathToFile);
                                }

                                if (File.Exists(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]))
                                {
                                    File.Delete(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]);
                                }

                                entry.ExtractToFile(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]);
                            }
                        }
                    }
                }
            }

            if (!isJ && !isN)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                ZipFile.ExtractToDirectory(archive, path);
            }

            if (arguments.Contains("-l"))
            {
                Console.WriteLine(GetFullStructure(archive));
            }

        }

        public static string GetFullStructure(string archive)
        {
            string structure = "Archive: " + archive + "\n" + "  Length\tDate Time\n";
            int count = 0;

            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
                {
                    count = zipArchive.Entries.Count;
                }

                DateTime dateTime = File.GetCreationTime(archive);

                structure += "  " + count + "\t\t" + dateTime;
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
            }

            return structure;
        }

        public static void UncompressFiles(string archive, string path, string file, List<string> arguments)
        {
            string[] fullFile = file.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string fileWithoutSlash = string.Join("", fullFile);
            bool isJ = arguments.Contains("-j") ? true : false;
            bool isN = arguments.Contains("-n") ? true : false;

            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
                {
                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                    {
                        string[] fullEntry = entry.ToString().Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        string newEntry = string.Join("", fullEntry);
                        string[] fileWithExtansion = fullEntry[fullEntry.Length - 1].Split('.');
                        string pathToFile = string.Empty;

                        if (fileWithoutSlash == newEntry)
                        {
                            for (int i = 0; i < fullEntry.Length - 1; i++)
                            {
                                pathToFile += fullEntry[i] + "\\";
                            }

                            if (isJ)
                            {
                                if (fileWithExtansion.Length >= 2)
                                {
                                    if (isN)
                                    {
                                        if (File.Exists(path + "\\" + fullEntry[fullEntry.Length - 1]))
                                        {
                                            Console.WriteLine("File " + path + "\\" + fullEntry[fullEntry.Length - 1] + " already exists");
                                            return;
                                        }
                                    }

                                    if (File.Exists(path + "\\" + fullEntry[fullEntry.Length - 1]))
                                    {
                                        File.Delete(path + "\\" + fullEntry[fullEntry.Length - 1]);
                                    }

                                    entry.ExtractToFile(path + "\\" + fullEntry[fullEntry.Length - 1]);
                                }
                            }
                            else
                            {
                                if (fileWithExtansion.Length >= 2)
                                {
                                    if (isN)
                                    {
                                        if (File.Exists(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]))
                                        {
                                            Console.WriteLine("File " + path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1] + " already exists");
                                            return;
                                        }
                                    }

                                    if (!Directory.Exists(path + "\\" + pathToFile))
                                    {
                                        Directory.CreateDirectory(path + "\\" + pathToFile);
                                    }

                                    if (File.Exists(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]))
                                    {
                                        File.Delete(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]);
                                    }

                                    entry.ExtractToFile(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]);
                                }
                            }
                        }
                    }
                }

                if (arguments.Contains("-l"))
                {
                    Console.WriteLine(GetFullStructure(archive));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                return;
            }
        }

        public static void UncompressDirectories(string archive, string path, string file, List<string> arguments)
        {
            string[] fullFile = file.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string fileWithoutSlash = string.Join("", fullFile);
            bool isJ = arguments.Contains("-j") ? true : false;
            bool isN = arguments.Contains("-n") ? true : false;

            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
                {
                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                    {
                        string[] fullEntry = entry.ToString().Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        string newEntry = string.Join("", fullEntry);
                        string[] fileWithExtansion = fullEntry[fullEntry.Length - 1].Split('.');
                        string pathToFile = string.Empty;

                        for (int i = 0; i < fullEntry.Length - 1; i++)
                        {
                            pathToFile += fullEntry[i] + "\\";
                        }

                        if (newEntry.StartsWith(fileWithoutSlash))
                        {
                            if (fileWithExtansion.Length >= 2)
                            {
                                if (isJ)
                                {
                                    if (isN)
                                    {
                                        if (File.Exists(path + "\\" + fullEntry[fullEntry.Length - 1]))
                                        {
                                            Console.WriteLine("File " + path + "\\" + fullEntry[fullEntry.Length - 1] + " already exists");
                                            return;
                                        }
                                    }

                                    if (File.Exists(path + "\\" + fullEntry[fullEntry.Length - 1]))
                                    {
                                        File.Delete(path + "\\" + fullEntry[fullEntry.Length - 1]);
                                    }

                                    entry.ExtractToFile(path + "\\" + fullEntry[fullEntry.Length - 1]);
                                }
                                else
                                {
                                    if (isN)
                                    {
                                        if (File.Exists(path + "\\" + fullEntry[fullEntry.Length - 1]))
                                        {
                                            Console.WriteLine("File " + path + "\\" + fullEntry[fullEntry.Length - 1] + " already exists");
                                            return;
                                        }
                                    }

                                    if (!Directory.Exists(path + "\\" + pathToFile))
                                    {
                                        Directory.CreateDirectory(path + "\\" + pathToFile);
                                    }

                                    if (File.Exists(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]))
                                    {
                                        File.Delete(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]);
                                    }

                                    entry.ExtractToFile(path + "\\" + pathToFile + "\\" + fullEntry[fullEntry.Length - 1]);
                                }
                            }
                        }
                    }
                }

                if (arguments.Contains("-l"))
                {
                    Console.WriteLine(GetFullStructure(archive));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                return;
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
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\unzip.txt"));
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

            if (_files.Count < 1)
            {
                Console.WriteLine("unzip: command takes values");
                return false;
            }

            return true;
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
    }
}
