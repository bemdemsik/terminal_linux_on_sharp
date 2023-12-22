using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class DiskFree
    {
        public static void PrintDiskFree(string command)
        {
            bool[] args = GetArgs(command);
            if (args[3])
            {
                Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\df.txt"));
                return;
            }
            string[] spl = command.Split(' ');
            string path = string.Empty;
            for(int i = 0; i<spl.Length; i++)
            {
                if(spl[i].Contains("\\"))
                {
                    path = spl[i];
                }
            }
            string output = string.Empty;
            var drives = new List<DriveInfo>();
            if (args[0])
            {
                drives.AddRange(DriveInfo.GetDrives());
            }
            else
            {
                if(string.IsNullOrEmpty(path))
                {
                    path = Directory.GetCurrentDirectory();
                }
                try
                {
                    var directory = new DirectoryInfo(path);
                    drives.Add(new DriveInfo(directory.FullName.Substring(0, 1)));
                }
                catch (Exception ex)
                {
                    output += "\n" + path + ": " + ex.Message;
                }
            }
            output += $"{"Name",6} {"Total",12} {"Used",12} {"Avaliable",12} {"Use",3}% Mounted on";
            foreach (var d in drives)
            {
                var total = d.TotalSize;
                var used = d.TotalSize - d.AvailableFreeSpace;
                var free = d.AvailableFreeSpace;
                var percent = (float)used / (float)total * 100;
                string unit = "";
                if (args[2])
                {
                    total = total / 1024;
                    used = used / 1024;
                    free = free / 1024;
                    unit = "KB";
                }
                else if (args[1])
                {
                    total = total / (1024 * 1024 * 1024);
                    used = used / (1024 * 1024 * 1024);
                    free = free / (1024 * 1024 * 1024);
                    unit = "GB";
                }
                output += $"\n{d.Name,6} {total + unit,12} {used + unit,12} {free + unit,12} {Math.Truncate(percent),3}% {d.DriveFormat}";
            }
            Console.WriteLine(output);
        }

        private static bool[] GetArgs(string command)
        {
            string[] splitLine = command.Split(' ');
            bool flagA = false;
            bool flagH = false;
            bool flagK = false;
            bool flagHelp = false;
            if (splitLine.Contains("-a"))
            {
                flagA = true;
            }
            if (splitLine.Contains("-human"))
            {
                flagH = true;
            }
            if (splitLine.Contains("-k"))
            {
                flagK = true;
            }
            if (splitLine.Contains("-h"))
            {
                flagHelp = true;
            }
            if (splitLine.Contains("--help"))
            {
                flagHelp = true;
            }
            bool[] args = {flagA, flagH, flagK, flagHelp};
            return args;
        }
    }
}
