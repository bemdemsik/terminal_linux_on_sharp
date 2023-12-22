using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class dirname
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 06.06.2023
        /// </summary>
        /// <param name="ndir"></param>
        /// <returns></returns>
        public static void Dirname(string command, string path)
        {
            command = command.Replace(@"\", @"/");
            string[] arr = command.Split(' ');
            string key1 = "";
            if (arr.Length > 1)
            {
                key1 = arr[1];
            }
            if (arr[arr.Length - 1] == "-z" || arr[arr.Length - 1] == "-zero")
            {
                Console.WriteLine("dirname: a file name not entered");
                return;
            }
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\dirname.txt"));
                    return;
                }
                if (key1 == "--version")
                {
                    string message = @"dirname (GNU coreutils) 8.28'";
                    Console.WriteLine(message);
                    return;
                }
                for (int i3 = 1; i3 < arr.Length; i3++)
                {
                    string ndir = "";
                    if (arr[i3] != "-z" && arr[i3] != "--zero")
                    {
                        ndir = arr[i3];
                    }
                    else
                    {
                        continue;
                    }
                    string[] dpath = arr[i3].Split('/');
                    if (!File.Exists(path + @"\" + arr[i3]) && !File.Exists(arr[i3]))
                    {
                        Console.WriteLine("dirname: cannot get directory name, '" + dpath[dpath.Length - 1] + "': No such file or directory");
                    }
                    else
                    {
                        string temp = arr[i3].Replace(@"/" + dpath[dpath.Length - 1], @"");
                        if (dpath[dpath.Length - 1] == temp)
                        {
                            if (key1 == "-z" || key1 == "--zero")
                            {
                                Console.Write(".");
                            }
                            else 
                            {
                                Console.WriteLine(".");
                            }
                        }
                        else
                        {
                            if (key1 == "-z" || key1 == "--zero")
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
            }
            catch (Exception ex)
            {

            }
        }
    }
}
