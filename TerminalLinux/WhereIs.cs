using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    public static class WhereIs
    {
        static List<string> _arguments = new List<string>() { "-b", "-l", "-B", "-h" };
        static List<string> _userArguments = new List<string>();
        static List<string> _files = new List<string>();
        static List<string> _folders = new List<string>();
        public static void FindAllFiles(string[] command)
        {
            if (command.Length == 1)
            {
                Console.WriteLine("whereis: the command takes values");
                return;
            }

            _folders.Clear();
            _userArguments.Clear();
            _files.Clear();

            if (!CheckArguments(command[0], command))
            {
                return;
            }

            Console.WriteLine(Search());

        }

        public static string Search()
        {
            bool isBinary = _userArguments.Contains("-b") ? true : false;

            string myFile = string.Empty;

            if (!isBinary)
            {
                myFile = _files[_files.Count - 1];
            }
            else
            {
                myFile = "*";
            }

            string rootFolder = @"D:\";
            List<string> files = new List<string>();

            if (!_userArguments.Contains("-B"))
            {
                TraverseFileSystem(rootFolder, ref files, myFile, isBinary);
            }
            else
            {
                GetFiles(_folders, myFile, ref files, isBinary);
            }

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            return "";

        }

        static void GetFiles(List<string> folders, string fileName, ref List<string> files, bool isBinary)
        {
            try
            {
                foreach (var value in folders)
                {
                    foreach (string file in Directory.GetFiles(value, fileName, SearchOption.AllDirectories))
                    {
                        if (!file.StartsWith(@"D:\$RECYCLE.BIN\"))
                        {
                            if (isBinary)
                            {
                                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                                byte[] buffer = new byte[1024];

                                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                                for (int i = 0; i < bytesRead; i++)
                                {
                                    if (buffer[i] < 32 || buffer[i] > 126)
                                    {
                                        files.Add(file);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                files.Add(file);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
            }
        }

        static void TraverseFileSystem(string folder, ref List<string> files, string fileName, bool isBinary)
        {
            try
            {
                foreach (string file in Directory.GetFiles(folder, fileName))
                {
                    if (!file.StartsWith(@"D:\$RECYCLE.BIN\"))
                    {
                        if (isBinary)
                        {
                            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                            byte[] buffer = new byte[1024];

                            int bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                            for (int i = 0; i < bytesRead; i++)
                            {
                                if (buffer[i] < 32 || buffer[i] > 126)
                                {
                                    files.Add(file);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            files.Add(file);
                        }

                    }
                }

                foreach (string subDir in Directory.GetDirectories(folder))
                {
                    if (!subDir.StartsWith(@"D:\$RECYCLE.BIN\"))
                    {
                        TraverseFileSystem(subDir, ref files, fileName, isBinary);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool CheckArguments(string command, string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                bool check = false;
                bool isBinary = false;

                if (input[i] == command)
                {
                    continue;
                }

                if (input[i].ToCharArray()[0] == '-')
                {
                    if (_arguments.Contains(input[i]))
                    {
                        if (!_userArguments.Contains(input[i]))
                        {

                            if (input[i] == "-h")
                            {
                                Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\whereis.txt"));
                                return false;
                            }

                            if (input[i] == "-b")
                            {
                                isBinary = true;
                            }

                            if (input[i] == "-B")
                            {
                                _userArguments.Add(input[i]);
                                check = true;

                                try
                                {
                                    while (!_arguments.Contains(input[i + 1]))
                                    {
                                        if (i == input.Length - 1 && _files.Count == 0 && !isBinary)
                                        {
                                            _files.Add(input[i]);
                                            break;
                                        }
                                        _folders.Add(input[i + 1]);
                                        i++;
                                    }

                                    if (_folders.Count == 0)
                                    {
                                        Console.WriteLine("whereis -B: after the argument, the directory(s) must be entered");
                                        return false;
                                    }
                                }
                                catch (Exception)
                                {
                                    if (_folders.Count == 0)
                                    {
                                        Console.WriteLine("whereis -B: after the argument, the directory(s) must be entered");
                                        return false;
                                    }
                                }
                            }

                            if (input[i] == "-l")
                            {
                                foreach (string subDir in Directory.GetDirectories(@"D:\"))
                                {
                                    if (!subDir.StartsWith(@"D:\$RECYCLE.BIN\"))
                                    {
                                        Console.WriteLine(subDir);
                                    }
                                }

                                return false;
                            }



                            if (!check)
                            {
                                _userArguments.Add(input[i]);
                            }

                        }
                    }
                }
                else
                {
                    _files.Add(input[i]);
                }
            }

            return true;
        }
    }
}
