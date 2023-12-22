using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TerminalLinux
{
    class Program
    {
        static void Main(string[] args)
        {
            EnviromentVariables.FillValiables();
            while (true)
            {
                string userText = ConsoleDesign.Start();
                history.AddCommandToDictionary(userText);
                string[] input = userText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (input.Length == 0)
                {
                    continue;
                }

                string command = input[0];

                switch (command)
                {
                    case "cd":
                        ChangeDirectory.Change(input);
                        break;
                    case "cowsay":
                        Cowsay.CowSay(input);
                        break;
                    case "rmdir":
                        rmdir.Rmdir(userText, Directory.GetCurrentDirectory());
                        break;
                    case "mkdir":
                        mkdir.Mkdir(userText, Directory.GetCurrentDirectory());
                        break;
                    case "touch":
                        touch.Touch(userText, Directory.GetCurrentDirectory());
                        break;
                    case "dirname":
                        dirname.Dirname(userText, Directory.GetCurrentDirectory());
                        break;
                    case "basename":
                        basename.Basename(userText, Directory.GetCurrentDirectory());
                        break;
                    case "rename":
                        rename.Rename(userText, Directory.GetCurrentDirectory());
                        break;
                    case "fortune":
                        fortune.Fortune(userText, Directory.GetCurrentDirectory());
                        break;
                    case "find":
                        find.Find(userText, Directory.GetCurrentDirectory());
                        break;
                    case "wc":
                        wc.Wc(userText, Directory.GetCurrentDirectory());
                        break;
                    case "pwd":
                        PrintWorkingDirectory.Print(input);
                        break;
                    case "uname":
                        Uname.PrintSystemInfo(userText);
                        break;
                    case "tail":
                        Tail.PrintResult(userText);
                        break;
                    case "cat":
                        Cat.Catenate(input);
                        break;
                    case "ps":
                        Processes.ShowProcesses(input);
                        break;
                    case "ls":
                        List.ExecuteList(userText, Directory.GetCurrentDirectory());
                        break;
                    case "head":
                        Head.ReadLines(userText, Directory.GetCurrentDirectory());
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
                        DiskFree.PrintDiskFree(userText);
                        break;
                    case "uptime":
                        Uptime.ShowTime(input);
                        break;
                    case "mv":
                        Move.Moving(input);
                        break;
                    case "echo":
                        echo.Echo(userText, Directory.GetCurrentDirectory());
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
                        history.RemoveHistoryCommand();
                        history.GetHistory(input);
                        break;
                    case "file":
                        FileCommand.TypeFile(input);
                        break;
                    case "mkpasswd":
                        Mkpasswd.GetHash(input);
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
                    case "whereis":
                        WhereIs.FindAllFiles(input);
                        break;
                    default:
                        Console.WriteLine("bash: command not found");
                        break;
                }
            }
        }
    }
}
