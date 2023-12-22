using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace TerminalLinux
{
    class Uniq
    {
        public static void ShowUniq(string[] arguments)
        {
            List<string> allFileArray = new List<string>();
            List<string> uniqArray = new List<string>();
            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-u");
            CommandLebedev.flagCommand.Add("-h");
            CommandLebedev.flagCommand.Add("-d");
            CommandLebedev.flagCommand.Add("-D");
            if (!CommandLebedev.Fill(arguments))
                return;
            if (CommandLebedev.values.Count == 0 || CommandLebedev.values.Count > 2)
            {
                Console.WriteLine("uniq: value: error syntax");
                return;
            }
            try
            {
                allFileArray.AddRange(File.ReadAllLines(Directory.GetCurrentDirectory() + "\\" + CommandLebedev.values[0]));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
                return;
            }
            if (CommandLebedev.flagEnter.Contains("-h"))
            {
                if (CommandLebedev.flagEnter.Count == 1 && CommandLebedev.values.Count == 0)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\uniq.txt"));
                else
                    Console.WriteLine("Invalid key -h");
                return;
            }
            if (CommandLebedev.flagEnter.Count == 0)
                foreach (string lines in allFileArray)
                    Console.WriteLine(lines);
            else if (CommandLebedev.flagEnter.Count > 1)
                Console.WriteLine("uniq: command cannot accept more than one option");
            else if (CommandLebedev.flagEnter.Contains("-u"))
            {
                foreach (string linesUniq in allFileArray)
                    if (allFileArray.IndexOf(linesUniq) == allFileArray.LastIndexOf(linesUniq))
                        uniqArray.Add(linesUniq);
            }
            else if (CommandLebedev.flagEnter.Contains("-d"))
            {
                foreach (string uniqLines in allFileArray)
                    if (!uniqArray.Contains(uniqLines))
                        uniqArray.Add(uniqLines);
            }
            else if (CommandLebedev.flagEnter.Contains("-D"))
            {
                foreach (string linesDontUniq in allFileArray)
                    if (allFileArray.IndexOf(linesDontUniq) != allFileArray.LastIndexOf(linesDontUniq))
                        uniqArray.Add(linesDontUniq);
            }
            if (CommandLebedev.values.Count == 1)
                foreach (string lines in uniqArray)
                    Console.WriteLine(lines);
            else
            {
                if (CommandLebedev.values[0].Split('.')[1] == CommandLebedev.values[1].Split('.')[1])
                    try
                    {
                        StreamWriter streamWriter = new StreamWriter(Directory.GetCurrentDirectory() + "\\" + CommandLebedev.values[1]);
                        if (CommandLebedev.flagEnter.Count != 0)
                            streamWriter.Write(string.Join("\n", uniqArray));
                        else
                            streamWriter.Write(string.Join("\n", allFileArray));
                        streamWriter.Close();
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("uniq: error " + CommandLebedev.values[1]);
                    }
                else Console.WriteLine("uniq: extensions don't match");
            }
        }
    }
}
