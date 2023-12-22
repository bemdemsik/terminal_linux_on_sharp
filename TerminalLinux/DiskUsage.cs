using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class DiskUsage
    {
        static List<string> dir= new List<string>();
        static List<string> getValues = new List<string>();
        static List<string> getValuesAndSize = new List<string>();
        static public void ExecuteDiskUsage(string[] command)
        {
            long totalInDir=0;
            dir.Clear();
            getValues.Clear();
            getValuesAndSize.Clear();
            Command.SplitCommand(command);
            Command.flag.Add("-a");
            Command.flag.Add("-h");
            Command.flag.Add("-c");
            Command.flag.Add("--help");
            if (!Command.CheckArguments())
            {
                Console.WriteLine("invalid flag");
                return;
            }
            if (Command.args.Contains("--help"))
            {
                if (Command.args.Count == 1)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\du.txt"));
                else
                    Console.WriteLine("Invalid key -h");
                return;
            }
            dir = Command.values;
            if (dir.Count <= 0)
                dir.Add(Directory.GetCurrentDirectory());
            foreach (var file in dir)
                try
                {
                    string current = string.Empty;
                    if (Path.IsPathRooted(dir[dir.Count - 1]) == false)
                        current = Directory.GetCurrentDirectory() + "\\";
                    getValues.Add(AddSize(current+file));
                    totalInDir+= ReSize(new DirectoryInfo(current + file));
                }
                catch (Exception ex)
                {
                    getValues.Add(" du: " + file + ": " + ex.Message); 
                    continue;
                }
            Console.WriteLine(string.Join("\n", getValuesAndSize));
            if (Command.args.Contains("-c"))
            {
                string size = "";
                if (Command.args.Contains("-h"))
                    size = (totalInDir / 1024).ToString() + "KB";
                else
                    size = totalInDir.ToString();
                Console.WriteLine($"\n {size,6}\ttotal");
            }
        }
        static private string AddSize(string path)
        {
            string output = "";
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                string size = "";
                if (Command.args.Contains("-h"))
                    size=(ReSize(directoryInfo) / 1024).ToString() + "KB";
                else
                    size = ReSize(directoryInfo).ToString();
                getValuesAndSize.Add(size + "\t" + path);
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    getValuesAndSize.Add(AddSize(directory.FullName));
                }
                if (Command.args.Contains("-a"))
                    foreach (var directory in directoryInfo.GetFiles())
                    {
                        getValuesAndSize.Add(AddSize(directory.FullName));
                    }
            }
            else if (File.Exists(path))
            {
                string size = Command.args.Contains("-h") ? (File.ReadAllBytes(path).Length / 1024).ToString() + "KB" : File.ReadAllBytes(path).Length.ToString();
                getValuesAndSize.Add(size + "\t" + path);
            }
            return output;
        }
        static private long ReSize(DirectoryInfo directory)
        {
            long size = 0;
            FileInfo[] fileInfos = directory.GetFiles();
            foreach (FileInfo fi in fileInfos)
                try
                {
                    size += fi.Length;
                }
                catch (UnauthorizedAccessException)
                {
                    ;
                }
            DirectoryInfo[] directoryInfos = directory.GetDirectories();
            foreach (DirectoryInfo directoryInfo in directoryInfos) 
                try
                {
                    size += ReSize(directoryInfo);
                }
                catch (UnauthorizedAccessException)
                {
                    ;
                }
            return size;
        }

    }
}
