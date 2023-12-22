using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    public static class Uptime
    {
        static List<string> _arguments = new List<string>() { "-s", "-p", "-h" };
        static List<string> _userArguments = new List<string>();

        public static void ShowTime(string[] commands)
        {
            _userArguments.Clear();

            if (commands.Length == 1)
            {
                Console.WriteLine(PrintDefaultTime());
                return;
            }

            if (!CheckArguments(commands[0], commands))
            {
                return;
            }

            Console.WriteLine(ShowWithArguments(_userArguments));

        }

        public static string ShowWithArguments(List<string> arguments)
        {
            var systemStartTime = GetSystemStartTime();

            string result = string.Empty;

            var diff = DateTime.Now - systemStartTime;
            var minutes = (int)diff.TotalMinutes;

            if (arguments.Contains("-p"))
            {
                result += "up " + minutes + " minutes ";
            }

            if (arguments.Contains("-s"))
            {
                result += systemStartTime;
            }

            return result;
        }

        public static bool CheckArguments(string command, string[] userInput)
        {
            foreach (var value in userInput)
            {
                if (value == command)
                {
                    continue;
                }

                if (value.ToCharArray()[0] == '-')
                {
                    if (_arguments.Contains(value))
                    {
                        if (value == "-h")
                        {
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\uptime.txt"));
                            return false;
                        }
                        if (!_userArguments.Contains(value))
                        {
                            _userArguments.Add(value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid key " + value);
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("uptime: does not take a values");
                    return false;
                }
            }

            return true;
        }

        [DllImport("kernel32.dll")]
        static extern uint GetTickCount();

        static DateTime GetSystemStartTime()
        {
            var ticks = GetTickCount();
            var uptime = TimeSpan.FromMilliseconds(ticks);
            var systemBootTime = DateTime.Now - uptime;
            return systemBootTime;
        }

        public static string PrintDefaultTime()
        {
            var systemStartTime = GetSystemStartTime();

            var diff = DateTime.Now - systemStartTime;
            var minutes = (int)diff.TotalMinutes;

            string now = DateTime.Now.ToShortDateString();
            string currentUsers = string.Empty;
            string loadAverage = "load average: ";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
    "SELECT * FROM Win32_UserAccount WHERE Disabled=false AND SIDType = 1");

            int count = 0;

            foreach (ManagementObject envVar in searcher.Get())
            {
                currentUsers += (count + 1).ToString() + " " + envVar["Name"] + ", ";
                count++;
            }

            currentUsers = currentUsers.Substring(0, currentUsers.Length - 2);

            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();

            Thread.Sleep(1000);
            var cpuUsage = cpuCounter.NextValue();

            loadAverage += cpuUsage;

            return now + "\n" + "up " + minutes + " min" + "\n" + currentUsers + "\n" + loadAverage;
        }
    }
}
