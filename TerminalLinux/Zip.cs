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
    public static class Zip
    {
        static string globalFolder = string.Empty;
        static string[] checkFolders = new string[0];
        static bool isD = false;
        static bool isR = false;
        static List<string> _arguments = new List<string>() { "-r", "-0", "-d", "-h" };
        static List<string> _files = new List<string>();
        static List<string> _userArguments = new List<string>();

        public static void Archive(string[] commands)
        {
            if (commands.Length == 1)
            {
                Console.WriteLine("zip: the command takes values");
                return;
            }

            globalFolder = string.Empty;
            checkFolders = new string[0];
            isD = false;
            isR = false;
            _files.Clear();
            _userArguments.Clear();

            if (!CheckArguments(commands[0], commands))
            {
                return;
            }

            string result = CheckEvent(_files, _userArguments);

            Console.WriteLine(result);

        }

        public static List<string> SetArguments(List<string> arguments)
        {
            List<string> newArguments = new List<string>();

            foreach (var value in arguments)
            {
                if (value == "-r" && !isD)
                {
                    isR = true;
                    newArguments.Add(value);
                }
                if (value == "-0")
                {
                    newArguments.Add(value);
                }

                if (value == "-d" && !isR)
                {
                    isD = true;
                    newArguments.Add(value);
                }
            }

            return newArguments;
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

        public static string CheckEvent(List<string> files, List<string> arguments)
        {
            string file = string.Empty;
            string directory = string.Empty;
            bool isDirectory = true;

            arguments = SetArguments(arguments);

            bool deleteFiles = arguments.Contains("-d") ? true : false;
            bool onlyFiles = arguments.Contains("-r") ? false : true;

            file = CheckPath(files[0]);

            directory = CheckDirectory(files[0]);

            if (directory != string.Empty)
            {
                return "The name of the archive must be specified";
            }

            string expansionFile = string.Empty;

            string[] fullPath = files[0].Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string fileWithExpansion = fullPath[fullPath.Length - 1];
            string pathWithoutFile = string.Empty;
            string expansion = string.Empty;

            for (int i = 0; i < fullPath.Length - 1; i++)
            {
                pathWithoutFile += fullPath[i] + "\\";
            }

            string[] checkExpansion = fileWithExpansion.Split('.');

            if (checkExpansion.Length >= 2)
            {
                expansion = checkExpansion[checkExpansion.Length - 1];
            }

            if (expansion != "zip")
            {
                return "zip: the name must contain the .zip extension";
            }

            if (fullPath.Length > 1)
            {
                isDirectory = true;
            }
            else
            {
                isDirectory = false;
            }

            if (isDirectory)
            {
                directory = CheckDirectory(pathWithoutFile);

                if (directory == string.Empty)
                {
                    return "Directory must be exist";
                }
            }

            string archive = directory + fileWithExpansion;

            if (!CreateArchive(archive))
            {
                return "Something went wrong";
            }

            int count = 0;

            foreach (var value in files)
            {
                if (count == 0)
                {
                    count++;
                    continue;
                }

                if (arguments.Contains("-d"))
                {
                    bool isFile = false;
                    string[] fullPathValue = value.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    string valueWithExpansion = fullPathValue[fullPathValue.Length - 1];
                    string valueWithoutFile = string.Empty;
                    string expansionValue = string.Empty;

                    for (int i = 0; i < fullPathValue.Length - 1; i++)
                    {
                        valueWithoutFile += fullPathValue[i] + "\\";
                    }

                    string[] checkExpansionValue = valueWithExpansion.Split('.');

                    if (checkExpansionValue.Length >= 2)
                    {
                        expansionValue = checkExpansionValue[checkExpansionValue.Length - 1];
                        isFile = true;
                    }
                    else
                    {
                        isFile = false;
                    }

                    DeleteFile(archive, value, isFile);
                    continue;
                }

                if (onlyFiles)
                {
                    string fileInArchive = CheckPath(value);

                    if (fileInArchive == string.Empty)
                    {
                        Console.WriteLine("File " + value + " does not exist");
                    }
                    else
                    {

                        FileArchivator(archive, fileInArchive, arguments);
                    }
                }
                else
                {
                    string checkDirectory = CheckDirectory(value);

                    if (checkDirectory == string.Empty)
                    {
                        Console.WriteLine("Directory " + value + " not found");
                    }
                    else
                    {
                        string[] folders = checkDirectory.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        globalFolder = folders[folders.Length - 1];
                        FolderArchivator(archive, checkDirectory, arguments);
                    }
                }

                count++;
            }

            return "";

        }

        public static void DeleteFile(string archive, string file, bool isFile)
        {
            bool checkDelete = false;

            int countEntries = 0;

            using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
            {
                if (isFile)
                {
                    string[] filePath = file.Replace('/', '\\').Split('\\');
                    file = string.Join("", filePath);

                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                    {
                        string[] pathEntry = entry.ToString().Replace('/', '\\').Split('\\');
                        string fullPathEntry = string.Join("", pathEntry);

                        if (fullPathEntry == file)
                        {
                            entry.Delete();
                            checkDelete = true;
                            break;
                        }
                    }

                    if (!checkDelete)
                    {
                        Console.WriteLine("File " + file + " not found in this archive");
                    }
                }
                else
                {
                    List<ZipArchiveEntry> entriesToDelete = new List<ZipArchiveEntry>();

                    foreach (var value in zipArchive.Entries)
                    {
                        string[] fullFile = file.Replace('/', '\\').Split('\\');
                        string currentFile = string.Join("", fullFile);
                        string[] fullValue = value.FullName.Replace('/', '\\').Split('\\');
                        string currentValue = string.Join("", fullValue);

                        if (currentValue.StartsWith(currentFile))
                        {
                            entriesToDelete.Add(value);
                        }
                    }

                    foreach (var entry in entriesToDelete)
                    {
                        using (var stream = entry.Open())
                        {
                            stream.SetLength(0);
                        }

                        entry.Delete();
                    }
                }

                countEntries = zipArchive.Entries.Count;
            }

            if (countEntries == 0)
            {
                try
                {
                    File.Delete(archive);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong");
                }
            }
        }

        public static void FolderArchivator(string archive, string folder, List<string> arguments)
        {
            string fullPathWithDrive = folder;
            string[] fullPath = folder.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            string nameFolder = fullPath[fullPath.Length - 1];
            string[] folders = Directory.GetDirectories(folder);
            string currentPath = string.Empty;

            if (checkFolders.Length == 0)
            {
                Array.Resize(ref checkFolders, checkFolders.Length + 1);
                checkFolders[checkFolders.Length - 1] = folder;
            }

            bool isGlobalFolder = false;

            for (int i = 0; i < fullPath.Length; i++)
            {
                if (isGlobalFolder)
                {
                    currentPath += fullPath[i] + "\\";
                }

                if (fullPath[i] == globalFolder)
                {
                    currentPath += fullPath[i] + "\\";
                    isGlobalFolder = true;
                }

            }

            using (var zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
            {
                var archiveContents = zipArchive.Entries.Select(e => e.FullName).ToList();

                if (!archiveContents.Contains(currentPath + "\\"))
                {
                    if (arguments.Contains("-0"))
                    {
                        zipArchive.CreateEntry(currentPath + "\\", CompressionLevel.NoCompression);
                    }
                    else
                    {
                        zipArchive.CreateEntry(currentPath + "\\", CompressionLevel.Optimal);
                    }

                }
            }

            foreach (string file in Directory.GetFiles(folder).Where(file => (File.GetAttributes(file) & FileAttributes.Hidden) == 0).ToArray())
            {
                string[] fileName = file.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (CheckExistFile(archive, currentPath + "\\" + fileName[fileName.Length - 1], fileName[fileName.Length - 1]))
                {
                    using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
                    {
                        if (arguments.Contains("-0"))
                        {
                            zipArchive.CreateEntryFromFile(folder + "\\" + fileName[fileName.Length - 1], currentPath + "\\" + fileName[fileName.Length - 1], CompressionLevel.NoCompression);
                        }
                        else
                        {
                            zipArchive.CreateEntryFromFile(folder + "\\" + fileName[fileName.Length - 1], currentPath + "\\" + fileName[fileName.Length - 1], CompressionLevel.Optimal);
                        }
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
                    FolderArchivator(archive, fullPathWithDrive + "\\" + path[path.Length - 1], arguments);
                    checkFolders[checkFolders.Length - 1] = folder + "\\" + path[path.Length - 1];
                }

                checkCount = 0;
            }
        }

        public static bool CheckExistFile(string archive, string folder, string fileName)
        {
            string[] folderFull = folder.Replace('/', '\\').Split('\\');
            folder = string.Join("", folderFull);
            using (ZipArchive zipArchive = ZipFile.OpenRead(archive))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    string[] fullEntry = entry.ToString().Replace('/', '\\').Split('\\');
                    string newEntry = string.Join("", fullEntry);

                    if (folder == newEntry)
                    {
                        if (entry.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("In this archive file " + string.Join("\\", folderFull) + " already exists");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static void FileArchivator(string archive, string file, List<string> arguments)
        {
            try
            {
                using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
                {
                    if (arguments.Contains("-0"))
                    {
                        zipArchive.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.NoCompression);
                    }
                    else
                    {
                        zipArchive.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("File " + file + " failed to archive");
                return;
            }
        }

        public static bool CreateArchive(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    using (ZipArchive zipArchive = ZipFile.Open(path, ZipArchiveMode.Create)) ;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
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
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\zip.txt"));
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
                    string[] path = value.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    string newPath = string.Join("", path);

                    if (newPath == "D:" || newPath == "C:")
                    {
                        Console.WriteLine("You cannot archive a disk");
                        return false;
                    }

                    _files.Add(value);
                }
            }

            if (_files.Count < 2)
            {
                Console.WriteLine("zip: takes 2 or more values");
                return false;
            }

            return true;
        }
    }
}
