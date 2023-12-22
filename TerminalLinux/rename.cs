using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class rename
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 06.06.2023
        /// </summary>
        /// <param name="fName"></param>
        /// <returns></returns>
        public static void Rename(string command, string path)
        {
            string[] arr = command.Split(' ');
            string key1 = "";
            string[] keys = { "-i", "--interactive", "-v", "--verbose", "-n", "--no-act" };
            bool inter = false;
            bool ver = false;
            bool nact = false;
            string rep = "";
            bool chek = false;
            if (arr.Length > 1)
            {
                key1 = arr[1];
                for (int i = 1; i < arr.Length; i++) 
                {
                    if (arr[i] == "-i" || arr[i] == "--interactive")
                    {
                        inter = true;
                    }
                    else if (arr[i] == "-v" || arr[i] == "--verbose")
                    {
                        ver = true;
                    }
                    else if (arr[i] == "-n" || arr[i] == "--no-act")
                    {
                        nact = true;
                    }
                    else if (!chek)
                    {
                        chek = true;
                        rep = arr[i];
                    }
                }
            }
            if (arr[arr.Length - 1] == "-i" || arr[arr.Length - 1] == "--interactive" || arr[arr.Length - 1] == "-v" || arr[arr.Length - 1] == "--verbose" || arr[arr.Length - 1] == "-n" || arr[arr.Length - 1] == "--no-act" || arr[arr.Length - 1] == rep)
            {
                Console.WriteLine("rename: a file name not entered");
                return;
            }
            rep = rep.Replace("'", "");
            string[] ren = rep.Split('/');
            if (ren.Length != 3)
            {
                Console.WriteLine("rename: incorrect syntax");
                return;
            }
            else if (ren[0] != "s")
            {
                Console.WriteLine("rename: incorrect syntax");
                return;
            }
            string wRep = ren[1];
            string nRep = ren[2];
            string fName = arr[arr.Length - 1];
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\rename.txt"));
                    return;
                }
                if (inter)
                {
                err1:
                    Console.WriteLine("rename: are you sure? (y/n)");
                    string chois = Console.ReadLine();
                    if (chois != "y" && chois != "n")
                    {
                        goto err1;
                    }
                    else if (chois == "n")
                    {
                        return;
                    }
                }
                foreach (string findedFile in Directory.EnumerateFiles(path, fName))
                {
                    FileInfo FI;
                    try
                    {
                        FI = new FileInfo(findedFile);
                        string oldName = FI.FullName;
                        string tPatch = oldName.Replace(@"\" + FI.Name, "");
                        string newName = tPatch + @"\" + FI.Name.Replace(wRep, nRep);
                        if (nact)
                        {
                            if (FI.Name != FI.Name.Replace(wRep, nRep))
                            {
                                Console.WriteLine("rename(" + FI.Name + ", " + FI.Name.Replace(wRep, nRep) + ")");
                            }
                        }
                        else if (ver)
                        {
                            if (FI.Name != FI.Name.Replace(wRep, nRep))
                            {
                                Console.WriteLine(FI.Name + " renamed as " + FI.Name.Replace(wRep, nRep));
                            }
                        }
                        if (!nact)
                        {
                            System.IO.File.Move(oldName, newName);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
