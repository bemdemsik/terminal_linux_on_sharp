using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class history
    {
        static Dictionary<int, string> commands = new Dictionary<int, string>();
        public static void GetHistory(string[] args)
        {
            try
            {
                if (args.Contains("-c"))
                {
                    commands.Clear();
                    return;
                }
                else
                {
                    if(args.Contains("-h") || args.Contains("--help"))
                    {
                        Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\history.txt"));
                        return;
                    }
                    if (EnviromentVariables.HistSize != 0)
                    {
                        for (int i = 0; i < commands.Count; i++)
                        {
                            string line = (i + 1) + " " + commands[i + 1];
                            Console.WriteLine(line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("0 ");
                        return;
                    }

                    EternityUserWrite();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occurred while executing the command. Check command syntax");
            }
        }

        private static void EternityUserWrite()
        {
            string lastCommand = commands[commands.Count];
            while (true)
            {
                Console.Write("$ ");
                var line = Console.ReadLine();
                if (line == "^exit")
                    break;
                if (line == "!!")
                {
                    Console.WriteLine(lastCommand);
                    ExecuteCommand(lastCommand);
                }
                else
                {
                    try
                    {
                        int number = GetNumber(line);
                        string command = commands[number];
                        lastCommand = command;
                        Console.WriteLine(command);
                        ExecuteCommand(command);
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("where is no line by this key");
                    }
                }
            }
        }

        public static void RemoveHistoryCommand()
        {
            commands.Remove(commands.Count);
        }

        private static void ExecuteCommand(string command)
        {
            string[] input = command.Split(' ');
            string currCmd = input[0];
            switch (currCmd)
            {
               case "cd":
                        ChangeDirectory.Change(input);
                        break;
                    case "cowsay":
                        Cowsay.CowSay(input);
                        break;
                    case "rmdir":
                        rmdir.Rmdir(command, Directory.GetCurrentDirectory());
                        break;
                    case "mkdir":
                        mkdir.Mkdir(command, Directory.GetCurrentDirectory());
                        break;
                    case "touch":
                        touch.Touch(command, Directory.GetCurrentDirectory());
                        break;
                    case "dirname":
                        dirname.Dirname(command, Directory.GetCurrentDirectory());
                        break;
                    case "basename":
                        basename.Basename(command, Directory.GetCurrentDirectory());
                        break;
                    case "rename":
                        rename.Rename(command, Directory.GetCurrentDirectory());
                        break;
                    case "fortune":
                        fortune.Fortune(command, Directory.GetCurrentDirectory());
                        break;
                    case "find":
                        find.Find(command, Directory.GetCurrentDirectory());
                        break;
                    case "wc":
                        wc.Wc(command, Directory.GetCurrentDirectory());
                        break;
                    case "pwd":
                        PrintWorkingDirectory.Print(input);
                        break;
                    case "uname":
                        Uname.PrintSystemInfo(command);
                        break;
                    case "tail":
                        Tail.PrintResult(command);
                        break;
                    case "cat":
                        Cat.Catenate(input);
                        break;
                    case "ps":
                        Processes.ShowProcesses(input);
                        break;
                    case "ls":
                        List.ExecuteList(command, Directory.GetCurrentDirectory());
                        break;
                    case "head":
                        Head.ReadLines(command, Directory.GetCurrentDirectory());
                        break;
                    case "du":
                        DiskUsage.ExecuteDiskUsage(input);
                        break;
                    case "arch":
                        Architecture.ExecuteArchitectue(input);
                        break;
                    case "kill":
                        Kill.KillProcess(input);
                        break;
                    case "hexdump":
                        Hexdump.HexDump(input);
                        break;
                    case "clear":
                        Clear.ClearConsole(input);
                        break;
                    case "reboot":
                        Reboot.RebootPC(input);
                        break;
                    case "exit":
                        Exit.ExitConsole(input);
                        break;
                    case "rm":
                        Remove.RemoveDirectoryOrFile(input);
                        break;
                    case "date":
                        Date.DatePrint(input);
                        break;
                    case "cp":
                        Copy.Copying(input);
                        break;
                    case "df":
                        DiskFree.PrintDiskFree(command);
                        break;
                    case "uptime":
                        Uptime.ShowTime(input);
                        break;
                    case "mv":
                        Move.Moving(input);
                        break;
                    case "echo":
                        echo.Echo(command, Directory.GetCurrentDirectory());
                        break;
                    case "man":
                        Man.ManualCommand(input);
                        break;
                    case "expand":
                        Expand.ReplaceTabToProb(input);
                        break;
                    case "factor":
                        Factor.PrintResult(input);
                        break;
                    case "whoami":
                        Whoami.ShowWhoAmI(input);
                        break;
                    case "shutdown":
                        Shutdown.ShutDown(input);
                        break;
                    case "zip":
                        Zip.Archive(input);
                        break;
                    case "grep":
                        Grep.GrepInFile(input);
                        break;
                    case "groups":
                        Groups.GroupsUsers(input);
                        break;
                    case "uniq":
                        Uniq.ShowUniq(input);
                        break;
                    case "pwgen":
                        Pwgen.PasswordGeneration(input);
                        break;
                    case "env":
                        Env.PrintValiables(input);
                        break;
                    case "history":
                    Console.WriteLine("You are already in history mode");
                        break;
                    case "file":
                        FileCommand.TypeFile(input);
                        break;
                    case "unzip":
                        Unzip.Uncompress(input);
                        break;
                    case "wget":
                        Wget.Download(input);
						break;
                    case "diff":
                        Diff.GetResultComparisons(input);
                        break;
                    default:
                        Console.WriteLine("bash: command not found");
                        break;
            }
        }

        private static int GetNumber(string line)
        {
            string numberStr = string.Empty;
            for(int i = 0; i<line.Length; i++)
            {
                if(Char.IsDigit(line[i]))
                {
                    numberStr += line[i];
                }
            }
            return Convert.ToInt32(numberStr);
        }

        public static void AddCommandToDictionary(string command)
        {
            try
            {
                int histSize = EnviromentVariables.HistSize;
                if (histSize >= commands.Count)
                {
                    commands.Add(commands.Count + 1, command);
                }
            }
            catch(Exception)
            {
                
            }
        }
    }
}
