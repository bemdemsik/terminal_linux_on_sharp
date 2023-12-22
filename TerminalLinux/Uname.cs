using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class Uname
    {
        private static bool[] GetArgsInfo(string args)
        {
            string[] splitLine = args.Split(' ');
            bool flagP = false;
            bool flagO = false;
            bool flagA = false;
            bool flagH = false;
            if (splitLine.Contains("-p"))
            {
                flagP = true;
            }
            if (splitLine.Contains("-o"))
            {
                flagO = true;
            }
            if (splitLine.Contains("-a"))
            {
                flagA = true;
            }
            if(splitLine.Contains("-h"))
            {
                flagH = true;
            }
            if (splitLine.Contains("--help"))
            {
                flagH = true;
            }
            bool[] flags = { flagP, flagO, flagA, flagH };
            return flags;
        }

        private static string GetAllSysInfo()
        {
            string[] str = new string[5];
            str[0] = Environment.OSVersion.ToString();
            if (System.Environment.Is64BitOperatingSystem)
            {
                str[1] = "64";
            }
            else
            {
                str[1] = "32";
            }
            str[2] = Environment.MachineName;
            str[3] = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            str[4] = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
            string strRes = str[0] + ", " + str[1] + ", " + str[2] + ", " + str[3] + ", " + str[4];
            return strRes;
        }



        public static void PrintSystemInfo(string args)
        {
            try
            {
                bool[] flags = GetArgsInfo(args);
                if (flags[3])
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\Uname.txt"));
                    return;
                }
                string res = "";
                if (flags[2])
                {
                    res += GetAllSysInfo();
                    Console.WriteLine(res);
                }
                if (flags[1])
                {
                    if (System.Environment.Is64BitOperatingSystem)
                    {
                        res += "64 ";
                    }
                    else
                    {
                        res += "32 ";
                    }
                }
                if (flags[0])
                {
                    res += Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"); ;
                }
                if (res == "")
                {
                    res = GetAllSysInfo();
                }
                Console.WriteLine(res);
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error while executing command. Check the manual");
            }

        }
    }
}
