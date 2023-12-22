using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TerminalLinux
{
    internal class rmdir
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 01.06.2023
        /// </summary>
        /// <param name="ndir"></param>
        /// <returns></returns>
        public static void Rmdir(string command, string path)
        {
            string[] arr = command.Split(' ');
            string key1 = "";
            string key2 = "";
            string key3 = "";
            if (arr.Length > 1)
            {
                key1 = arr[1];
                key2 = arr[1];
                key3 = arr[1];
            }
            if (arr.Length > 2)
            {
                key2 = arr[2];
                key3 = arr[2];
            }
            if (arr.Length > 3)
            {
                key3 = arr[3];
            }
            if (arr[arr.Length - 1] == "-p" || arr[arr.Length - 1] == "-v" || arr[arr.Length - 1] == "mkdir" || arr[arr.Length - 1] == "--parents" || arr[arr.Length - 1] == "--verbose" || arr[arr.Length - 1] == "--ignore-fail-on-non-empty")
            {
                Console.WriteLine("rmdir: a directory name not entered");
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
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\rmdir.txt"));
                    return;
                }
                if (key1 == "--version")
                {
                    string message = @"rmdir (GNU coreutils) 8.32'";
                    Console.WriteLine(message);
                    return;
                }
                if (s)
                {
                    for (int i3 = 1; i3 < arr.Length; i3++)
                    {
                        if (arr[i3] != "-p" && arr[i3] != "-v" && arr[i3] != "--parents" && arr[i3] != "--verbose" && arr[i3] != "--ignore-fail-on-non-empty")
                        {
                            ndir = arr[i3];
                        }
                        else
                        {
                            continue;
                        }
                        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path + @"/" + ndir);
                        if (dir.Exists)
                        {
                            if (!Directory.EnumerateFileSystemEntries(path + @"/" + ndir).Any() || key1 == "--ignore-fail-on-non-empty")
                            {
                                dir.Delete(true);
                                if (key2 == "-v" || key2 == "--verbose" || key1 == "-v" || key1 == "--verbose" || key3 == "-v" || key3 == "--verbose")
                                {
                                    Console.WriteLine(@"rmdir: removing directory, '" + ndir + "'");
                                }
                            }
                            else
                            {
                                Console.WriteLine(@"rmdir: failed to remove '" + ndir + "': Directory not empty");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine(@"rmdir: cannot removing directory, '" + ndir + "': No such file or directory");
                        }
                    }
                }
                else if (p)
                {
                    if (key1 == "-p" || key1 == "--parents" || key2 == "-p" || key2 == "--parents")
                    {
                        string d = "";
                        for (int i2 = 0; i2 < dpath.Length - 1; i2++)
                        {
                            path += @"/" + dpath[i2];
                            d += dpath[i2] + @"/";

                        }
                        path += @"/" + dpath[dpath.Length - 1];
                        d += dpath[dpath.Length - 1];
                        for (int i = dpath.Length - 1; i >= 0; i--)
                        {
                            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                            if (dir.Exists)
                            {
                                if (!Directory.EnumerateFileSystemEntries(path).Any() || key1 == "--ignore-fail-on-non-empty")
                                {
                                    dir.Delete(true);
                                    if (key2 == "-v" || key2 == "--verbose" || key1 == "-v" || key1 == "--verbose" || key3 == "-v" || key3 == "--verbose")
                                    {
                                        Console.WriteLine(@"rmdir: removing directory, '" + d + "'");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(@"rmdir: failed to remove '" + d + "': Directory not empty");
                                    return;
                                }

                            }
                            else
                            {
                                Console.WriteLine(@"rmdir: cannot removing directory, '" + d + "': No such file or directory");
                            }
                            path = path.Replace(@"/" + dpath[i], "");
                            d = d.Replace(@"/" + dpath[i], "");
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
                        path += @"/" + dpath[dpath.Length - 1];
                        d += dpath[dpath.Length - 1];
                        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                        if (dir.Exists)
                        {
                            if (!Directory.EnumerateFileSystemEntries(path).Any() || key1 == "--ignore-fail-on-non-empty")
                            {
                                dir.Delete(true);
                                if (key2 == "-v" || key2 == "--verbose" || key1 == "-v" || key1 == "--verbose" || key3 == "-v" || key3 == "--verbose")
                                {
                                    Console.WriteLine(@"rmdir: removing directory, '" + d + "'");
                                }
                            }
                            else
                            {
                                Console.WriteLine(@"rmdir: failed to remove '" + d + "': Directory not empty");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine(@"rmdir: cannot removing directory, '" + d + "': No such file or directory");
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
