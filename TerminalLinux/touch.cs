using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TerminalLinux
{
    internal class touch
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 01.06.2023
        /// </summary>
        /// <param name="ndir"></param>
        /// <returns></returns>
        public static void Touch(string command, string path)
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
            if (arr[arr.Length - 1] == "-a" || arr[arr.Length - 1] == "-c" || arr[arr.Length - 1] == "--no-create" || arr[arr.Length - 1] == "-m")
            {
                Console.WriteLine("touch: a file name not entered");
                return;
            }
            string ndir = arr[arr.Length - 1];
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\touch.txt"));
                    return;
                }
                for (int i3 = 1; i3 < arr.Length; i3++)
                {
                    if (arr[i3] != "-a" && arr[i3] != "-c" && arr[i3] != "--no-create" && arr[i3] != "-m")
                    {
                        ndir = arr[i3];
                    }
                    else
                    {
                        continue;
                    }
                    if (!File.Exists(path + @"/" + ndir) && key1 != "-c" && key1 != "--no-create")
                    {
                        using (File.Create(path + @"/" + ndir + ".txt"))
                        {
                            // Чтобы в дальнейшем файл можно было удалить без перезапуска программы
                        }
                    }
                    if (key2 == "-a" || key1 == "-a")
                    {
                        System.IO.File.SetLastAccessTime(path + @"/" + ndir, DateTime.Now);
                    }
                    if (key2 == "-m" || key1 == "-m")
                    {
                        System.IO.File.SetLastWriteTime(path + @"/" + ndir, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
