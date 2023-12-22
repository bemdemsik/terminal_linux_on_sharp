using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class find
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 03.06.2023
        /// </summary>
        /// <param name="fName"></param>
        /// <returns></returns>
        public static void Find(string command, string path)
        {
            string[] arr = command.Split(' ');
            string[] keys = { "-name", "-iname", "-empty" , "-type"};
            string pPath = "";
            string fName = "*";
            bool uReg = false;
            bool emt = false;
            string key1 = "";
            bool dir = true;
            bool fil = true;
            if (arr.Length > 1)
            {
                pPath = arr[1];
                key1 = arr[1];
                for (int i = 0; i < arr.Length; i++)
                {
                    string key = arr[i];
                    if (key == "-name")
                    {
                        uReg = true;
                        if ((i + 1) < arr.Length)
                        {
                            fName = arr[i + 1].Replace("\"", "");
                        }
                        else
                        {
                            Console.WriteLine("find: a file name not entered");
                            return;
                        }
                    }
                    if (key == "-iname")
                    {
                        if ((i + 1) < arr.Length)
                        {
                            fName = arr[i + 1].Replace("\"", "");
                        }
                        else
                        {
                            Console.WriteLine("find: a file name not entered");
                            return;
                        }
                    }
                    if (key == "-empty")
                    {
                        emt = true;
                    }
                    if (key == "-type")
                    {
                        if ((i + 1) < arr.Length)
                        {
                            if (arr[i + 1] != "f" && arr[i + 1] != "d")
                            {
                                Console.WriteLine("find: a type not entered");
                                return;
                            }
                            else if (arr[i + 1] == "f")
                            {
                                dir = false;
                                fil = true;
                            }
                            else if (arr[i + 1] == "d")
                            {
                                dir = true;
                                fil = false;
                            }
                        }
                        else
                        {
                            Console.WriteLine("find: a type not entered");
                            return;
                        }
                    }
                }
            }
            if (pPath != "." && !keys.Contains(pPath))
            {
                path += pPath;
            }
            string[] arrs = pPath.Split('/');
            {
                if (arrs[0] == "~" && arrs.Length == 1)
                {
                    path = @"C:";
                }
                else if (arrs[0] == "~")
                {
                    path = @"C:\";
                    for (int i = 1; i < arrs.Length; i++)
                    {
                        if ((i + 1) == arrs.Length)
                        {
                            path += arrs[i];
                        }
                        else
                        {
                            path += arrs[i] + @"\";
                        }
                    }
                }
            }
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\find.txt"));
                    return;
                }
                path = path.Replace(@"/", @"\");
                path = path.Replace(@".", @"");
                if (dir)
                {
                    foreach (string dirsInDir in Directory.GetDirectories(path, fName, SearchOption.AllDirectories))
                    {
                        DirectoryInfo FI;
                        try
                        {
                            FI = new DirectoryInfo(dirsInDir);
                            if (Directory.EnumerateFileSystemEntries(FI.FullName).Any() && emt)
                            {
                                continue;
                            }
                            int check = FI.Name.CompareTo(fName);
                            if (uReg && fName.Length != FI.Name.Length)
                            {
                                string FN = FI.FullName.Replace(path, ".");
                                Console.WriteLine(FN.Replace(@"\", @"/") + @"/");
                            }
                            else if (uReg && check == 0)
                            {
                                string FN = FI.FullName.Replace(path, ".");
                                Console.WriteLine(FN.Replace(@"\", @"/") + @"/");
                            }
                            else if (!uReg)
                            {
                                string FN = FI.FullName.Replace(path, ".");
                                Console.WriteLine(FN.Replace(@"\", @"/") + @"/");
                            }
                        }
                        catch
                        {
                            continue;
                        }

                    }
                }
                if (fil)
                {
                    foreach (string findedFile in Directory.EnumerateFiles(path, fName, SearchOption.AllDirectories))
                    {
                        FileInfo FI;
                        try
                        {
                            FI = new FileInfo(findedFile);
                            if (FI.Length != 0 && emt)
                            {
                                continue;
                            }
                            int check = FI.Name.CompareTo(fName);
                            if (uReg && fName.Length != FI.Name.Length)
                            {
                                string FN = FI.FullName.Replace(path, ".");
                                Console.WriteLine(FN.Replace(@"\", @"/"));
                            }
                            else if (uReg && check == 0)
                            {
                                string FN = FI.FullName.Replace(path, ".");
                                Console.WriteLine(FN.Replace(@"\", @"/"));
                            }
                            else if (!uReg)
                            {
                                string FN = FI.FullName.Replace(path, ".");
                                Console.WriteLine(FN.Replace(@"\", @"/"));
                            }
                        }
                        catch
                        {
                            continue;
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
