using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class mkdir
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 01.06.2023
        /// </summary>
        /// <param name="ndir"></param>
        /// <returns></returns>
        public static void Mkdir(string command, string path)
        {
            string[] arr = command.Split(' ');
            string key1 = "";
            string key2 = "";
            if (arr.Length > 1)
            {
                key1 = arr[1];
                key2 = arr[1];
            }
            if (arr.Length > 2)
            {
                key2 = arr[2];
            }
            if (arr[arr.Length - 1] == "-p" || arr[arr.Length - 1] == "-v" || arr[arr.Length - 1] == "mkdir" || arr[arr.Length - 1] == "--parents" || arr[arr.Length - 1] == "--verbose")
            {
                Console.WriteLine("mkdir: a directory name not entered");
                return;
            }
            bool p = false;
            bool s = true;
            string ndir = arr[arr.Length - 1];
            string[] dpath = arr[arr.Length - 1].Split('/');
            if (dpath.Length > 1)
            {
                p = true;
                s = false;
                ndir = dpath[dpath.Length - 1];
            }
            string pd = "";

            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\mkdir.txt"));
                    return;
                }
                if (key1 == "--version")
                {
                    string message = @"mkdir (GNU coreutils) 8.32'";
                    Console.WriteLine(message);
                    return;
                }
                if (s)
                {
                    for (int i3 = 1; i3 < arr.Length; i3++)
                    {
                        if (arr[i3] != "-p" && arr[i3] != "-v" && arr[i3] != "--parents" && arr[i3] != "--verbose")
                        {
                            ndir = arr[i3];
                        }
                        else
                        {
                            continue;
                        }
                        if (key1 == "-p" || key1 == "--parents")
                        {
                            if (key2 == "-v" || key2 == "--verbose")
                            {
                                if (!Directory.Exists(path + @"/" + ndir))
                                {
                                    Directory.CreateDirectory(path + @"/" + ndir);
                                    Console.WriteLine(@"mkdir: created directory '" + ndir + "'");
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(path + @"/" + ndir))
                                {
                                    Directory.CreateDirectory(path + @"/" + ndir);
                                }
                            }
                        }
                        else if (Directory.Exists(ndir))
                        {
                            Console.WriteLine("mkdir: a directory with this name already exists");
                        }
                        else
                        {
                            Directory.CreateDirectory(path + @"/" + ndir);
                            if (key1 == "-v" || key1 == "--verbose")
                            {
                                Console.WriteLine(@"mkdir: created directory '" + ndir + "'");
                            }
                        }
                    }
                }
                else if (p)
                {
                    if (key1 == "-p" || key1 == "--parents")
                    {
                        for (int i = 0; i < dpath.Length; i++)
                        {
                            if (key2 == "-v" || key2 == "--verbose")
                            {
                                if (!Directory.Exists(path + @"/" + dpath[i]))
                                {
                                    Directory.CreateDirectory(path + @"/" + dpath[i]);
                                    Console.WriteLine(@"mkdir: created directory '" + pd + dpath[i] + "'");
                                    pd += dpath[i] + @"/";
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(path + @"/" + dpath[i]))
                                {
                                    Directory.CreateDirectory(path + @"/" + dpath[i]);
                                }
                            }
                            path += @"/" + dpath[i];
                        }
                    }
                    else
                    {
                        string d = "";
                        for (int i2 = 0; i2 < dpath.Length - 1; i2++)
                        {
                            path += @"/" + dpath[i2];
                            d += dpath[i2] + @"/";

                        }
                        if (!Directory.Exists(path))
                        {
                            Console.WriteLine("mkdir: cannot create directory '"+ d + dpath[dpath.Length - 1] + "': No such file or directory");
                            return;
                        }
                        if (Directory.Exists(path + @"/" + dpath[dpath.Length - 1]))
                        {
                            Console.WriteLine("mkdir: a directory with this name already exists");
                        }
                        else
                        {
                            Directory.CreateDirectory(path + @"/" + dpath[dpath.Length - 1]);
                            if (key1 == "-v" || key1 == "--verbose")
                            {
                                Console.WriteLine(@"mkdir: created directory '" + ndir + "'");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
