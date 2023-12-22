using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class Expand
    {
        public static void ReplaceTabToProb(string[] args)
        {
            try
            {
                bool[] flags = GetFlags(args);
                if (flags[3])
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\expand.txt"));
                    return;
                }
                int countFile = 0;
                if (!flags[2])
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Contains("."))
                        {
                            countFile++;
                            string currLines = ReadFile(args);
                            string result = removeProbels(currLines, flags[0], flags[1], args);
                            Console.WriteLine(result);
                            break;
                        }
                    }
                    if(countFile == 0)
                    {
                        string userLine = EternityUserWrite();
                        string result = removeProbels(userLine, flags[0], flags[1], args);
                        Console.WriteLine(result);
                    }
                }
                else
                {
                    int countFiles = 0;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Contains("."))
                        {
                            countFiles++;
                        }
                    }
                    bool secondFile = false;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Contains("."))
                        {
                            if (secondFile)
                            {
                                string currLines2 = ReadFile(args);
                                string result2 = removeProbels(currLines2, flags[0], flags[1], args);
                                WriteFile(args[i], result2);
                                return;
                            }
                            string currLines = ReadFile(args);
                            string result = removeProbels(currLines, flags[0], flags[1], args);
                            secondFile = true;
                        }
                        else
                        {
                            if (args[i] == ">" && countFiles == 1)
                            {
                                string userLine = EternityUserWrite();
                                string result = removeProbels(userLine, flags[0], flags[1], args);
                                WriteFile(args[i + 1], result);
                            }
                        }
                    }

                }
            }
            catch(Exception)
            {
                Console.WriteLine("An error occurred while executing the command. Check command syntax");
                return;
            }
        }

        private static string EternityUserWrite()
        {
            var listLines = new List<string>();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "^exit")
                    break;
                listLines.Add(line);
            }
            return string.Join("\r\n", listLines);
        }

        private static void WriteFile(string fileName, string result)
        {
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + fileName))
            {
                sw.WriteLine(result);
            }
        }

        private static string ReadFile(string[] args)
        {
            string fileName = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("."))
                {
                    fileName = args[i];
                    break;
                }
            }
            using (StreamReader sr = new StreamReader(fileName))
            {
                return sr.ReadToEnd();
            }
        }

        private static string removeProbels(string value, bool flagI, bool flagT, string[] args)
        {
            string result = string.Empty;
            string probel = " ";
            string concatProbel = " ";
            if(flagT)
            {
                int countT = 0;
                for(int i = 0; i<args.Length; i++)
                {
                    if(args[i].Contains("-t"))
                    {
                        countT = Convert.ToInt32(args[i + 1]);
                    }
                }
                if(countT == 0)
                {
                    probel = "";
                }
                else
                {
                    for (; countT > 1; countT--)
                    {
                        probel += concatProbel;
                    }
                }
            }
            if (flagI)
            {
                for(int i = 0; i<value.Length; i++)
                {
                    if (value[i] == '\t')
                    {
                        if (i != 0)
                        {
                            if (value[i - 1] == '\n' || value[i - 1] == '\t')
                            {
                                result += probel;
                            }
                            else
                            {
                                result += value[i];
                            }
                        }
                        else
                        {
                            result += probel;
                        }
                    }
                    else
                    {
                        result += value[i];
                    }
                }
            }
            else
            {
                result = value.Replace("\t", probel);
            }
            return result;
        }

        private static bool[] GetFlags(string[] args)
        {
            bool flagI = false;
            bool flagT = false;
            bool flagP = false;
            bool flagH = false;
            if(args.Contains("-i"))
            {
                flagI = true;
            }
            if (args.Contains("-t"))
            {
                flagT = true;
            }
            if (args.Contains(">"))
            {
                flagP = true;
            }
            if (args.Contains("-h"))
            {
                flagH = true;
            }
            if (args.Contains("--help"))
            {
                flagH = true;
            }
            return new bool[] { flagI, flagT, flagP, flagH};
        }
    }
}
