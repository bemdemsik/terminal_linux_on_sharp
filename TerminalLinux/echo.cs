using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class echo
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 05.06.2023
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static void Echo(string command, string path)
        {
            string[] arr = command.Split(' ');
            string key1 = "";
            if (arr.Length > 1)
            {
                key1 = arr[1];
            }
            string[] arrFW = command.Split('>');
            bool wr = false;
            bool r = false;
            if (arrFW.Length > 1)
            {
                wr = true;
                if (arrFW.Length > 2)
                {
                    wr = false;
                    r = true;
                }
            }
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\echo.txt"));
                    return;
                }
                char[] aС = key1.ToCharArray();
                if (aС[0] == '*')
                {
                    foreach (string dirsInDir in Directory.GetDirectories(path, key1, SearchOption.AllDirectories))
                    {
                        DirectoryInfo FI;
                        try
                        {
                            FI = new DirectoryInfo(dirsInDir);
                            Console.Write(FI.Name + " ");
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    foreach (string findedFile in Directory.EnumerateFiles(path, key1, SearchOption.AllDirectories))
                    {
                        FileInfo FI;
                        try
                        {
                            FI = new FileInfo(findedFile);
                            Console.Write(FI.Name + " ");
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    Console.WriteLine("");
                    return;
                }
                string inputText = "";
                if (key1 == "-e")
                {
                    string tempS = arr[2];
                    tempS = tempS.Replace(@"\\", @"\");
                    tempS = tempS.Replace(@"\n", Environment.NewLine);
                    tempS = tempS.Replace(@"\t", @"   ");
                    inputText += tempS;
                    for (int i = 3; i < arr.Length; i++)
                    {
                        if (arr[i] == ">")
                        {
                            break;
                        }
                        if (arr[i] == ">>")
                        {
                            break;
                        }
                        tempS = tempS = arr[i];
                        tempS = tempS.Replace(@"\\", @"\");
                        tempS = tempS.Replace(@"\n", Environment.NewLine);
                        tempS = tempS.Replace(@"\t", @"   ");
                        inputText += " " + tempS;
                    }
                }
                else if (key1 == "-E")
                {
                    inputText += arr[2];
                    for (int i = 3; i < arr.Length; i++)
                    {
                        if (arr[i] == ">")
                        {
                            break;
                        }
                        if (arr[i] == ">>")
                        {
                            break;
                        }
                        inputText += " " + arr[i];
                    }
                }
                else
                {
                    inputText += arr[1];
                    for (int i = 2; i < arr.Length; i++)
                    {
                        if (arr[i] == ">")
                        {
                            break;
                        }
                        if (arr[i] == ">>")
                        {
                            break;
                        }
                        inputText += " " + arr[i];
                    }
                }
                bool chek = false;
                if (wr || r)
                {
                    string[] arrC;
                    if (arrFW.Length > 2)
                    {
                        arrC = arrFW[2].Split('\\');
                    }
                    else
                    {
                        arrC = arrFW[1].Split('\\');
                    }
                    if (arrC.Length > 1)
                    {
                        chek = true;
                    }
                }
                if (wr)
                {
                    if (!File.Exists(path + @"\" + arrFW[1].Replace(@" ", @"")) && !chek)
                    {
                        using (File.Create(path + @"\" + arrFW[1].Replace(@" ", @"")))
                        {
                        }
                    }
                    else if (!File.Exists(arrFW[1].Replace(@" ", @"")) && chek)
                    {
                        using (File.Create(arrFW[1].Replace(@" ", @"")))
                        {
                        }
                    }
                    if (File.Exists(path + @"\" + arrFW[1].Replace(@" ", @"")) && !chek)
                    {
                        using (StreamWriter writer = new StreamWriter(path + @"\" + arrFW[1].Replace(@" ", @""), false))
                        {
                            writer.WriteLineAsync(inputText.Replace("\"", @""));
                        }
                    }
                    else if (File.Exists(arrFW[1].Replace(@" ", @"")) && chek)
                    {
                        using (StreamWriter writer = new StreamWriter(arrFW[1].Replace(@" ", @""), false))
                        {
                            writer.WriteLineAsync(inputText.Replace("\"", @""));
                        }
                    }
                }
                else if (r)
                {
                    if (!File.Exists(path + @"\" + arrFW[2].Replace(@" ", @"")) && !chek)
                    {
                        using (File.Create(path + @"\" + arrFW[2].Replace(@" ", @"")))
                        {
                        }
                    }
                    else if (!File.Exists(arrFW[2].Replace(@" ", @"")) && chek)
                    {
                        using (File.Create(arrFW[2].Replace(@" ", @"")))
                        {
                        }
                    }
                    if (File.Exists(path + @"/" + arrFW[2].Replace(@" ", @"")) && !chek)
                    {
                        using (StreamWriter writer = new StreamWriter(path + @"\" + arrFW[2].Replace(@" ", @""), true))
                        {
                            writer.WriteAsync(inputText.Replace("\"", @""));
                        }
                    }
                    else if (File.Exists(arrFW[2].Replace(@" ", @"")) && chek)
                    {
                        using (StreamWriter writer = new StreamWriter(arrFW[2].Replace(@" ", @""), true))
                        {
                            writer.WriteAsync(inputText.Replace("\"", @""));
                        }
                    }
                }
                else
                {
                    Console.WriteLine(inputText.Replace("\"", @""));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
