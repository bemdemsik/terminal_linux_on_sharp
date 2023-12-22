using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
namespace TerminalLinux
{
    class Man
    {
        static private string[] allFile;
        static private int specifiedLines = 0;
        static private int lenInfo = 0;
        static int topPosition = Console.CursorTop + 2;
        static public void ManualCommand(string[] arguments)
        {
            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-f");
            if (!CommandLebedev.Fill(arguments))
                return;
            try
            {
                if (CommandLebedev.values.Count <= 0)
                {
                    string[] allFiles = Directory.GetFiles(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\");
                    foreach(string file in allFiles)
                    {
                        string[] allLinesInFile = File.ReadLines(file).Skip(1).ToArray();
                        foreach(string lines in allLinesInFile)
                            if(lines.Contains("SYNOPSIS") || lines.Contains("DESCRIPTION") || lines == "")
                            {
                                Console.WriteLine();
                                break;
                            }
                            else Console.WriteLine(lines.Trim());
                    }
                    return;
                }
                string fileName = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\" + CommandLebedev.values[0] + ".txt";
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Command " + CommandLebedev.values[0] + " not found");
                    return;
                }
                allFile = File.ReadAllLines(fileName);
                if (CommandLebedev.flagEnter.Contains("-f"))
                    lenInfo = 2;
                else lenInfo = allFile.Length;
                Console.CursorVisible = false;
                ConsoleClear();
                ShowInfo();
                while (true)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Q: 
                            { 
                                ConsoleClear(); 
                                Console.CursorVisible = true; 
                                return; 
                            }
                        case ConsoleKey.UpArrow: specifiedLines -= specifiedLines > 0 ? 1 : 0; break;
                        case ConsoleKey.DownArrow: specifiedLines += specifiedLines < lenInfo - Console.WindowHeight + 1 ? 1 : 0; break;
                        default: break;
                    }
                    ConsoleClear();
                    ShowInfo();
                    Thread.Sleep(20);
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong");
                return;
            }
        }

        static void ShowInfo()
        {
            Console.SetCursorPosition(0, topPosition);
            for (int i = specifiedLines; i < specifiedLines + Console.WindowHeight - 1; i++)
                if (i > lenInfo - 1)
                    Console.Write(new string(' ', Console.WindowWidth - 1) + "\n");
                else Console.WriteLine((i + 1).ToString() + ". " + allFile[i]);
            Console.SetCursorPosition(0, topPosition);
        }
        static void ConsoleClear()
        {
            Console.SetCursorPosition(0, topPosition);
            for (int i = 0; i < Console.WindowHeight - 1; i++)
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\n\r");
            Console.SetCursorPosition(0, topPosition);
        }
    }
}
