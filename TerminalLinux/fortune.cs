using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class fortune
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 07.06.2023
        /// </summary>
        /// <param name="ndir"></param>
        /// <returns></returns>
        public static void Fortune(string command, string path)
        {
            string[] arr = command.Split(' ');
            string key1 = "";
            int numS = 160;
            bool lon = false;
            bool shor = false;
            bool cow = false;
            bool chek = false;
            string[] arrC = command.Split('|');
            string tem = "";
            bool chekT = false;
            if (arr.Length > 1)
            {
                key1 = arr[1];
                if (arrC.Length > 1)
                {
                    string[] arrC2 = arrC[1].Split(' ');
                    if ((arrC2[0] == "cowsay") || (arrC2[0] == "" && arrC2[1] == "cowsay"))
                    {
                        cow = true;
                    }
                }
                for (int i = 1; i < arr.Length; i++)
                {
                    if (arr[i] == "|")
                    {
                        break;
                    }
                    if (arr[i] == "-n")
                    {
                        if ((i + 1) < arr.Length)
                        {
                            try
                            {
                                numS = Convert.ToInt32(arr[i+1]);
                                i++;
                            }
                            catch
                            {
                                Console.WriteLine("fortune: incorrect syntax");
                                return;
                            }
                        }

                    }
                    else if (arr[i] == "-l" && !chek)
                    {
                        chek = true;
                        lon = true;
                    }
                    else if (arr[i] == "-s" && !chek)
                    {
                        chek = true;
                        shor = true;
                    }
                    else
                    {
                        chekT = true;
                        tem = arr[i];
                    }
                }
            }
            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\fortune.txt"));
                    return;
                }
                path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
                path += @"\fortune";
                string[] readText = { };
                foreach (string findedFile in Directory.EnumerateFiles(path))
                {
                    FileInfo FI;
                    try
                    {
                        FI = new FileInfo(findedFile);
                        if (chekT)
                        {
                            if (tem != FI.Name)
                            {
                                continue;
                            }
                        }
                        string Name = FI.FullName;
                        readText = readText.Concat(File.ReadAllLines(Name)).ToArray();
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                string s = string.Join("\n", readText);
                string[] arrT = s.Split('%');
            newF:
                Random rnd = new Random();
                int nCit = rnd.Next(0, arrT.Length - 1);
                if (lon)
                {
                    if (arrT[nCit].Length <= numS)
                    {
                        goto newF;
                    }
                }
                if (shor)
                {
                    if (arrT[nCit].Length > numS)
                    {
                        goto newF;
                    }
                }
                if (cow)
                {
                    string inputT = arrC[1];
                    inputT += " " + arrT[nCit];
                    string[] input = inputT.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    Cowsay.CowSay(input);
                }
                else
                {
                    Console.WriteLine(arrT[nCit]);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
