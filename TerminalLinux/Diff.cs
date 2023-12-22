using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class Diff
    {
        public static void GetResultComparisons(string[] args)
        {
            try
            {
                if (args.Contains("-h") || args.Contains("--help"))
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\diff.txt"));
                    return;
                }
                string result = ReadAndReturnEquals(args);
                Console.WriteLine(result);
            }
            catch(Exception)
            {
                Console.WriteLine("An error occurred while executing the command. Check command syntax");
            }
        }

        private static string[] GetFileNames(string[] args)
        {
            string fileName1 = string.Empty;
            string fileName2 = string.Empty;
            bool flag = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("."))
                {
                    if (flag)
                    {
                        fileName2 = args[i];
                    }
                    else
                    {
                        fileName1 = args[i];
                        flag = true;
                    }
                }
            }
            return new string[] { fileName1, fileName2 };
        }

        private static string ReadAndReturnEquals(string[] args)
        {
            bool[] flags = GetFlagsValue(args);
            string resultstr = string.Empty;
            string[] files = GetFileNames(args);
            string fileName1 = files[0];
            string fileName2 = files[1];
            string[] linesFile1;
            string[] linesFile2;
            string lines = string.Empty;

            using (StreamReader sr = new StreamReader(fileName1))
            {
                lines = sr.ReadToEnd();
                lines = lines.Replace("\r", "");
                linesFile1 = lines.Split('\n');
            }
            using (StreamReader sr = new StreamReader(fileName2))
            {
                lines = sr.ReadToEnd();
                lines = lines.Replace("\r", "");
                linesFile2 = lines.Split('\n');
            }
            if(flags[0])
            {
                return GetResWithflagC(linesFile1, linesFile2, flags[2], flags[1], fileName1, fileName2);
            }
            if (flags[2] || flags[1])
            {
                return GetResWithflag(linesFile1, linesFile2, fileName1, fileName2, flags[2], flags[1]);
            }
            return EqualsFile(linesFile1, linesFile2);
        }

        private static string GetResWithflagC(string[] linesFile1, string[] linesFile2, bool Qflag, bool Sflag, string fileName1, string fileName2)
        {
            if(Qflag)
            {
                return GetResWithflag(linesFile1, linesFile2, fileName1, fileName2, Qflag, Sflag);
            }
            string resulTrue = string.Empty;
            int countDiff = 0;
            if (Sflag)
            {
                resulTrue = string.Format("Files {0} and {1} differ by ", fileName1, fileName2);
            }
            int diffCount1 = 0;
            int diffCount2 = 0;
            string symbolP = "!";
            string symbolN = " ";
            string diffLinesFile1 = string.Empty;
            string diffLinesFile2 = string.Empty;
            bool flag = false;

            for (int i = 0; i < linesFile1.Length || i < linesFile2.Length; i++)
            {
                string line1 = string.Empty;
                string line2 = string.Empty;
                try
                { line1 = linesFile1[i]; }

                catch (Exception)
                { }
                try
                { line2 = linesFile2[i]; }
                catch (Exception)
                { }
                if (!string.Equals(line1, line2))
                {
                    countDiff++;
                    if (flag)
                    {
                        diffCount2 = i+1;
                        try
                        { if (line1 != "") diffLinesFile1 += symbolP + line1 + "\n"; }
                        catch (Exception) { }
                        try
                        { if (line2 != "") diffLinesFile2 += symbolP + line2 + "\n"; }
                        catch (Exception) { }

                    }
                    else
                    {
                        diffCount1 = i+1;
                        diffCount2 = i+1;
                        try
                        { if (line1 != "") diffLinesFile1 += symbolP + line1 + "\n"; }
                        catch (Exception)
                        { }
                        try
                        { if (line2 != "") diffLinesFile2 += symbolP + line2 + "\n"; }
                        catch (Exception)
                        { }
                        flag = true;
                    }
                }
                else
                {
                    try
                    { diffLinesFile1 += symbolN + line1 + "\n"; }
                    catch (Exception) { }
                    try
                    { diffLinesFile2 += symbolN + line2 + "\n"; }
                    catch (Exception) { }
                }
            }
            string result = string.Empty;
            if (diffCount1 != 0 && diffCount2 != 0)
            {
                result += "*** " + fileName1 + "\n";
                result += "--- " + fileName2 + "\n**********\n";
                result += "***" + diffCount1 + "," + diffCount2 + "***\n";
                string[] arr1 = diffLinesFile1.Split('\n');
                string[] arr2 = diffLinesFile2.Split('\n');
                foreach (string str in arr1)
                {
                    result += str + "\n";
                }
                result += "---" + diffCount1 + "," + diffCount2 + "---\n";
                foreach (string str in arr2)
                {
                    result += str + "\n";
                }
                if(Sflag)
                {
                    double percent = 0;
                    if (countDiff != 0)
                    {
                        int length = linesFile1.Length > linesFile2.Length ? linesFile1.Length : linesFile2.Length;
                        percent = (Convert.ToDouble(countDiff) / Convert.ToDouble(length)) * 100;
                        resulTrue += percent + "%";
                    }
                    result += resulTrue;
                }
            }
            return result;
        }

        private static string GetResWithflag(string[] linesFile1, string[] linesFile2, string fileName1, string fileName2, bool Qflag, bool Sflag)
        {
            string resulFalse;
            string resulTrue;
            int countDiff = 0;
            if (!Sflag)
            {
                resulFalse = string.Format("Files {0} and {1} differ", fileName1, fileName2);
                resulTrue = string.Format("Files {0} and {1} not differ", fileName1, fileName2);
            }
            else
            {
                resulFalse = string.Format("Files {0} and {1} differ by ", fileName1, fileName2);
                resulTrue = resulFalse;
            }
           
            for (int i = 0; i < linesFile1.Length || i < linesFile2.Length; i++)
            {
                string line1 = string.Empty;
                string line2 = string.Empty;
                try
                { line1 = linesFile1[i]; }

                catch (Exception)
                { }
                try
                { line2 = linesFile2[i]; }
                catch (Exception)
                { }
                if (!string.Equals(line1, line2))
                {
                    if(Sflag)
                    {
                        countDiff++;
                    }
                    else
                    {
                        return resulFalse;
                    }
                }
            }
            if(Sflag)
            {
                double percent = 0;
                if (countDiff != 0)
                {
                    int length = linesFile1.Length > linesFile2.Length ? linesFile1.Length : linesFile2.Length;
                    percent = (Convert.ToDouble(countDiff) / Convert.ToDouble(length)) * 100;
                }
                resulTrue += percent + "%";
            }
            return resulTrue;
        }

        private static string EqualsFile(string[] linesFile1, string[] linesFile2)
        {
            int diffCount1 = 0;
            int diffCount2 = 0;
            string symbolP = ">";
            string symbolM = "<";
            string diffLinesFile1 = string.Empty;
            string diffLinesFile2 = string.Empty;
            bool flag = false;

            for(int i = 0; i<linesFile1.Length || i<linesFile2.Length; i++)
            {
                string line1 =string.Empty;
                string line2 = string.Empty;
                try
                { line1 = linesFile1[i]; }

                catch (Exception)
                { }
                try
                { line2 = linesFile2[i]; }
                catch(Exception)
                { }
                if(!string.Equals(line1, line2))
                {
                    if(flag)
                    {
                        diffCount2 = i+1;
                        try
                        {  if (line1 != "") diffLinesFile1 += (line2 == "" ? symbolP : symbolM) + line1 + "\n"; }
                        catch(Exception) { }
                        try
                        { if (line2 != "") diffLinesFile2 += (line1 == "" ? symbolP : symbolM) + line2 + "\n"; }
                        catch (Exception) { }
                            
                    }
                    else
                    {
                        diffCount1 = i+1;
                        diffCount2 = i+1;
                        try
                        { if (line1 != "") diffLinesFile1 += symbolP + line1 + "\n"; }
                        catch(Exception)
                        { }
                        try
                        { if (line2 != "") diffLinesFile2 += symbolM + line2 + "\n"; }
                        catch(Exception)
                        { }
                        flag = true;
                    }
                }
            }
            string result = string.Empty;
            if(diffCount1 != 0 && diffCount2 != 0)
            {
                result = diffCount1 + "c" + diffCount2 + "\n";
                string[] arr1 = diffLinesFile1.Split('\n');
                string[] arr2 = diffLinesFile2.Split('\n');
                foreach(string str in arr1)
                {
                    result += str + "\n";
                }
                result += "---\n\n";
                foreach (string str in arr2)
                {
                    result += str + "\n";
                }
            }
            return result;
        }

        private static bool[] GetFlagsValue(string[] args)
        {
            bool flagC = false;
            bool flagS = false;
            bool flagQ = false;

            if(args.Contains("-c"))
            {
                flagC = true;
            }
            if (args.Contains("-s"))
            {
                flagS = true;
            }
            if (args.Contains("-q"))
            {
                flagQ = true;
            }
            return new bool[] { flagC, flagS, flagQ };
        }
    }
}
