using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class basename
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 06.06.2023
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public static void Basename(string command, string path)
        {
            command = command.Replace(@"\", @"/");
            string[] arr = command.Split(' ');
            string key1 = "";
            string pPath = "";
            string[] keys = { "-z", "--zero", "-s", "--suffix", "-a", "--multiple" };
            bool zer = false;
            bool suf = false;
            bool mul = false;
            string sufi = ".";
            if (arr.Length > 1)
            {
                pPath = arr[1];
                key1 = arr[1];
                for (int i = 0; i < arr.Length; i++)
                {
                    string key = arr[i];
                    if (key == "-z" || key == "-zero")
                    {
                        zer = true;
                    }
                    if (key == "-s" || key == "--suffix")
                    {
                        if ((i + 1) < arr.Length)
                        {
                            sufi += arr[i + 1].Replace(".", "");
                            suf = true;
                        }
                        else
                        {
                            Console.WriteLine("basename: a suffix not entered");
                            return;
                        }
                    }
                    if (key == "-a" || key == "--multiple")
                    {
                        mul = true;
                    }
                }
            }
            if (arr[arr.Length - 1] == "-z" || arr[arr.Length - 1] == "--zero" || arr[arr.Length - 1] == "-s" || arr[arr.Length - 1] == "--suffix" || arr[arr.Length - 1] == "-a" || arr[arr.Length - 1] == "--multiple")
            {
                Console.WriteLine("basename: a file name not entered");
                return;
            }
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\basename.txt"));
                    return;
                }
                if (key1 == "--version")
                {
                    string message = @"basename (GNU coreutils) 8.28'";
                    Console.WriteLine(message);
                    return;
                }
                if (mul)
                {
                    for (int i3 = 1; i3 < arr.Length; i3++)
                    {
                        string ndir = "";
                        if (arr[i3] != "-z" && arr[i3] != "--zero" && arr[i3] != "-s" && arr[i3] != "--suffix" && arr[i3] != "-a" && arr[i3] != "--multiple")
                        {
                            ndir = arr[i3];
                        }
                        else
                        {
                            if (arr[i3] == "-s" || arr[i3] == "--suffix")
                            {
                                i3++;
                            }
                            continue;
                        }
                        string[] dpath = arr[i3].Split('/');
                        if (!File.Exists(path + @"\" + arr[i3]) && !File.Exists(arr[i3]))
                        {
                            Console.WriteLine("basename: cannot get directory name, '" + dpath[dpath.Length - 1] + "': No such file or directory");
                        }
                        else
                        {
                            string temp = dpath[dpath.Length - 1];
                            if (suf)
                            {
                                temp = temp.Replace(sufi, "");
                            }
                            if (zer)
                            {
                                Console.Write(temp);
                            }
                            else
                            {
                                Console.WriteLine(temp);
                            }
                        }
                    }
                }
                else
                {
                    string[] dpath = arr[arr.Length - 1].Split('/');
                    if (!File.Exists(path + @"\" + arr[arr.Length - 1]) && !File.Exists(arr[arr.Length - 1]))
                    {
                        Console.WriteLine("basename: cannot get directory name, '" + dpath[dpath.Length - 1] + "': No such file or directory");
                    }
                    else
                    {
                        string temp = dpath[dpath.Length - 1];
                        if (suf)
                        {
                            temp = temp.Replace(sufi, "");
                        }
                        if (zer)
                        {
                            Console.Write(temp);
                        }
                        else
                        {
                            Console.WriteLine(temp);
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
