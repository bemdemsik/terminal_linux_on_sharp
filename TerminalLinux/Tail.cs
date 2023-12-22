using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class Tail
    {
        private static bool[] GetArgsInfo(string args)
        {
            string[] splitLine = args.Split(' ');
            bool flagN = false;
            bool flagQ = false;
            bool flagV = false;
            bool flagH = false;
            if(splitLine.Contains("-n"))
            {
                flagN = true;
            }
            if (splitLine.Contains("-q"))
            {
                flagQ = true;
            }
            if (splitLine.Contains("-v"))
            {
                flagV = true;
            }
            if (splitLine.Contains("-h"))
            {
                flagH = true;
            }
            if (splitLine.Contains("--help"))
            {
                flagH = true;
            }
            bool[] flags = {flagN, flagQ, flagV, flagH};
            return flags;
        }

        private static string GetLinesFromFile(string fileName, int n, bool flagQ, bool flagV, int count)
        {
            try
            {
                if(fileName == null)
                {
                    return string.Empty;
                }
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\" + fileName))
                {
                    string allLines = sr.ReadToEnd();
                    string[] lines = allLines.Split('\n');
                    if (n > lines.Length)
                    {
                        n = lines.Length;
                    }
                    string res = "";
                    if(count == 2 || flagV)
                    {
                        if (!flagQ)
                        {
                            res = "==> " + fileName + " <==" + "\n";
                        }
                        if (flagV && !fileName.Contains(fileName))
                        {
                            res = "==> " + fileName + " <==" + "\n";
                        }
                    }
                    
                    Array.Reverse(lines);
                    for(int i = 0; i<n; i++)
                    {
                        res += lines[i] + "\n";
                    }
                    return res;
                }
            }
            catch(Exception ex)
            {
                return "An error accuping while reading file";
            }
        }

        private static string[] GetFileName(string command)
        {
            try
            {
                string[] splitLine = command.Split(' ');
                string[] files = new string[2];
                int count = 0;
                for (int i = 0; i < splitLine.Length; i++)
                {
                    if (splitLine[i].Contains("."))
                    {

                        files[count] = splitLine[i];
                        count++;
                    }
                }
                return files;
                throw new Exception();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Current file not found or nit exist");
                return new string[0];
            }
            
        }

        public static void PrintResult(string command)
        {
            try
            {
                bool[] flags = GetArgsInfo(command);
                if (flags[3])
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\tail.txt"));
                    return;
                }
                int n = 10;
                int z = 0;
                while (z != 2)
                {
                    string[] files = GetFileName(command);
                    int count = 1;
                    if (files[z] == "")
                    {
                        return;
                    }
                    if(files[1] != "")
                    {
                        count = 2;
                    }

                    if (flags[0])
                    {
                        string[] splitLine = command.Split(' ');
                        for (int i = 0; i < splitLine.Length; i++)
                        {
                            if (splitLine[i] == "-n")
                            {
                                n = Convert.ToInt32(splitLine[i + 1]);
                            }
                        }
                    }
                    Console.Write(GetLinesFromFile(files[z], n, flags[1], flags[2], count));
                    z++;
                }
            }
            catch(Exception)
            {
                Console.WriteLine("An error while executing command. Check the manual");
            }
        }
    }
}
