using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    class wc
    {
        /// <summary>
        /// Vakhrushin Evgeniy, 02.06.2023
        /// </summary>
        /// <param name="ndir"></param>
        /// <returns></returns>
        public static void Wc(string command, string path)
        {
            string[] arr = command.Split(' ');
            string[] keys = { "-c", "--bytes", "-m", "--chars", "-l", "--lines", "-w", "--words" };
            string key1 = "";
            string key2 = "";
            string key3 = "";
            string key4 = "";
            if (arr.Length > 1)
            {
                key1 = arr[1];
                key2 = arr[1];
                key3 = arr[1];
                key4 = arr[1];
            }
            if (arr.Length > 2)
            {
                key2 = arr[2];
                key3 = arr[2];
                key4 = arr[2];
            }
            if (arr.Length > 3)
            {
                key3 = arr[3];
                key4 = arr[3];
            }
            if (arr.Length > 4)
            {
                key4 = arr[4];
            }
            if (keys.Contains(arr[arr.Length - 1]))
            {
                Console.WriteLine("wc: a file name not entered");
                return;
            }
            string ndir = arr[arr.Length - 1];

            try
            {
                if (key1 == "--help" || key1 == "-h")
                {
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\wc.txt"));
                    return;
                }
                int bytT = 0;
                int chT = 0;
                int linT = 0;
                int worT = 0;
                int count = 0;
                int count2 = 0;
                bool bys = false;
                bool chs = false;
                bool lins = false;
                bool wors = false;
                for (int i3 = 1; i3 < arr.Length; i3++)
                {
                    int byt = -1;
                    int ch = -1;
                    int lin = -1;
                    int wor = -1;
                    if (!keys.Contains(arr[i3]))
                    {
                        ndir = arr[i3];
                    }
                    else
                    {
                        continue;
                    }
                    if (!File.Exists(path + @"/" + ndir))
                    {
                        Console.WriteLine(@"wc: cannot count file, '" + ndir + "': No such file or directory");
                        return;
                    }
                    if (!keys.Contains(key1))
                    {
                        byt = 0;
                        ch = 0;
                        lin = 0;
                        wor = 0;
                        string[] lines = File.ReadAllLines(path + @"/" + ndir);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            lin++;
                            linT++;
                        }
                        byte[] bytes = File.ReadAllBytes(path + @"/" + ndir);
                        byt = bytes.Length;
                        bytT += bytes.Length;
                        using (var reader = new StreamReader(path + @"/" + ndir, detectEncodingFromByteOrderMarks: true))
                        {
                            while (reader.Read() > -1)
                            {
                                ch++;
                                chT++;
                            }
                        }
                        string ts = "";
                        using (var srs = new StreamReader(path + @"/" + ndir, detectEncodingFromByteOrderMarks: true))
                        {
                            while (srs.EndOfStream != true)
                            {
                                ts += srs.ReadLine();
                            }
                        }
                        string[] textMass = ts.Split(' ');
                        wor = textMass.Length;
                        worT += textMass.Length;
                        Console.WriteLine(@"" + lin + " " + wor + " " + byt + " " + ch + " " + ndir + "");
                        count++;
                        continue;
                    }
                    if (key1 == "-c" || key1 == "--bytes" || key2 == "-c" || key2 == "--bytes" || key3 == "-c" || key3 == "--bytes" || key4 == "-c" || key4 == "--bytes")
                    {
                        byt = 0;
                        byte[] bytes = File.ReadAllBytes(path + @"/" + ndir);
                        byt = bytes.Length;
                        bytT += bytes.Length;
                        bys = true;
                    }
                    if (key1 == "-m" || key1 == "--chars" || key2 == "-m" || key2 == "--chars" || key3 == "-m" || key3 == "--chars" || key4 == "-m" || key4 == "--chars")
                    {
                        ch = 0;
                        using (var reader = new StreamReader(path + @"/" + ndir, detectEncodingFromByteOrderMarks: true))
                        {
                            while (reader.Read() > -1)
                            {
                                ch++;
                                chT++;
                            }
                        }
                        chs = true;
                    }
                    if (key1 == "-l" || key1 == "--lines" || key2 == "-l" || key2 == "--lines" || key3 == "-l" || key3 == "--lines" || key4 == "-l" || key4 == "--lines")
                    {
                        lin = 0;
                        string[] lines = File.ReadAllLines(path + @"/" + ndir);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            lin++;
                            linT++;
                        }
                        lins = true;
                    }
                    if (key1 == "-w" || key1 == "--words" || key2 == "-w" || key2 == "--words" || key3 == "-w" || key3 == "--words" || key4 == "-w" || key4 == "--words")
                    {
                        wor = 0;
                        string ts = "";
                        using (var srs = new StreamReader(path + @"/" + ndir, detectEncodingFromByteOrderMarks: true))
                        {
                            while (srs.EndOfStream != true)
                            {
                                ts += srs.ReadLine();
                            }
                        }
                        string[] textMass = ts.Split(' ');
                        wor = textMass.Length;
                        worT += textMass.Length;
                        wors = true;
                    }
                    string fin = "";
                    if (byt != -1)
                    {
                        fin += byt + " ";
                    }
                    if (ch != -1)
                    {
                        fin += ch + " ";
                    }
                    if (lin != -1)
                    {
                        fin += lin + " ";
                    }
                    if (wor != -1)
                    {
                        fin += wor + " ";
                    }
                    fin += ndir;
                    Console.WriteLine(fin);
                    count2++;
                }
                if (count > 1)
                {
                    Console.WriteLine(@"" + linT + " " + worT + " " + bytT + " " + chT + " total");
                }
                if (count2 > 1)
                {
                    string fst = "";
                    if (bys)
                    {
                        fst += bytT + " ";
                    }
                    if (chs)
                    {
                        fst += chT + " ";
                    }
                    if (lins)
                    {
                        fst += linT + " ";
                    }
                    if (wors)
                    {
                        fst += worT + " ";
                    }
                    Console.WriteLine(fst + "total");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
