using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
namespace TerminalLinux
{
    public static class Remove
    {
        static public void RemoveDirectoryOrFile(string[] arguments)
        {
            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-d");
            CommandLebedev.flagCommand.Add("-r");
            CommandLebedev.flagCommand.Add("-v");
            CommandLebedev.flagCommand.Add("-f");
            CommandLebedev.flagCommand.Add("-h");
            if (!CommandLebedev.Fill(arguments))
                return;
            if (arguments.Length == 1)
            {
                Console.WriteLine(arguments[0] + ": error syntax");
                return;
            }
            if (CommandLebedev.flagEnter.Contains("-h"))
            {
                if (CommandLebedev.flagEnter.Count == 1 && CommandLebedev.values.Count == 0)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\rm.txt"));
                else
                    Console.WriteLine("Invalid key -h");
                return;
            }
            string currentDirectory = Directory.GetCurrentDirectory() + "\\";
            if (!CommandLebedev.flagEnter.Contains("-f"))
            {
                Console.WriteLine("Are you sure you want to delete this?");
                while (true)
                {
                    Console.Write("Y/N: ");
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (ConsoleKey.Y == key)
                        break;
                    else if (ConsoleKey.N == key)
                    {
                        Console.WriteLine();
                        return;
                    }
                    else
                        Console.WriteLine();
                }
                Console.WriteLine();
            }
            try
            {
                if (string.Concat(arguments).Contains("*"))
                {
                    if (arguments[1] == "*")
                    {
                        foreach (string file in Directory.GetFiles(currentDirectory, arguments[1]))
                            File.Delete(file);
                        foreach (string file in Directory.GetDirectories(currentDirectory))
                            Directory.Delete(file);
                    }
                    else
                        try
                        {
                            foreach (string path in Directory.GetFiles(currentDirectory, arguments[1]))
                            {
                                File.Delete(path);
                                if (CommandLebedev.flagEnter.Contains("-v"))
                                    Console.WriteLine("Remove " + Path.GetFileName(path));
                            }
                        }
                        catch (IOException)
                        {
                            Console.WriteLine("There is no access to this file. Try again later");
                            return;
                        }
                }
                else if (CommandLebedev.flagEnter.Contains("-d"))
                {
                    string[] directory = Directory.GetDirectories(currentDirectory);
                    foreach (string path in directory)
                        if (Directory.GetDirectories(path).Length + Directory.GetFiles(path).Length == 0)
                        {
                            Directory.Delete(path);
                            if (CommandLebedev.flagEnter.Contains("-v"))
                                Console.WriteLine("Removed " + Path.GetDirectoryName(path) + " empty directories");
                        }
                }
                else
                    foreach (string directoryOrFile in CommandLebedev.values)
                    {
                        if (Directory.Exists(currentDirectory + directoryOrFile))
                            if (CommandLebedev.flagEnter.Contains("-r"))
                                Directory.Delete(currentDirectory + directoryOrFile, true);
                            else Directory.Delete(currentDirectory + directoryOrFile);
                        else if (File.Exists(currentDirectory + directoryOrFile))
                            File.Delete(currentDirectory + directoryOrFile);
                        else
                            Console.WriteLine("Directory or file " + directoryOrFile + " not found");
                        if (CommandLebedev.flagEnter.Contains("-v"))
                            Console.WriteLine("Remove " + directoryOrFile);
                    }
            }
            catch (IOException)
            {
                Console.WriteLine("There is no access to this file. Try again later");
            }
            catch
            {
                Console.WriteLine("Something went wrong");
            }
        }
    }
}
