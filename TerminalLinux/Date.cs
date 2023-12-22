using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Globalization;

namespace TerminalLinux
{
    static public class Date
    {
        static int currentDay = (int)DateTime.Now.DayOfWeek;
        static int specifiedDay = 0;
        static string dateFormat = string.Empty;
        static int index = 0;
        static public void DatePrint(string[] arguments)
        {
            dateFormat = "ddd dd MMM yyyy HH-mm-ss K";
            if (arguments.Length == 1)
            {
                Console.WriteLine(DateTime.Now.ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US")));
                return;
            }

            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-f");
            CommandLebedev.flagCommand.Add("-s");
            CommandLebedev.flagCommand.Add("-h");
            if (!CommandLebedev.Fill(arguments))
                return;

            if (string.Concat(CommandLebedev.values).Contains('%'))
            {
                Console.WriteLine(string.Join(" ", CommandLebedev.values).Replace("\"", "").Replace("'", "")
                    .Replace("%Y", DateTime.Now.ToString(" yyyy "))
                    .Replace("%m", DateTime.Now.ToString(" MMM ", CultureInfo.CreateSpecificCulture("en-US")))
                    .Replace("%u", DateTime.Now.ToString(" ddd ", CultureInfo.CreateSpecificCulture("en-US")))
                    .Replace("%d", DateTime.Now.ToString(" dd ")).Trim());
                return;
            }
            else if (CommandLebedev.values.Count > 0 && CommandLebedev.flagEnter.Count == 0)
            {
                Console.WriteLine(arguments[0] + ": value: error value " + CommandLebedev.values[0]);
                return;
            }
            else if(CommandLebedev.values.Count > 1 && CommandLebedev.flagEnter.Count == 1 && CommandLebedev.flagEnter.Contains("-f"))
            {
                Console.WriteLine(arguments[0] + ": value: error value " + CommandLebedev.values[1]);
                return;
            }
            else if(CommandLebedev.values.Count > 2 && CommandLebedev.flagEnter.Count == 1 && CommandLebedev.flagEnter.Contains("-s"))
            {
                Console.WriteLine(arguments[0] + ": value: error value " + CommandLebedev.values[2]);
                return;
            }
            else if (CommandLebedev.values.Count > 3 && CommandLebedev.flagEnter.Count == 2)
            {
                Console.WriteLine(arguments[0] + ": value: error value " + CommandLebedev.values[3]);
                return;
            }
            if (CommandLebedev.flagEnter.Contains("-h"))
            {
                if (CommandLebedev.flagEnter.Count == 1 && CommandLebedev.values.Count == 0)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\date.txt"));
                else
                    Console.WriteLine("Invalid key -h");
                return;
            }

            if (CommandLebedev.flagEnter.Contains("-f"))
            {
                try
                {
                    dateFormat = string.Join(" ", CommandLebedev.values).Split('"')[1];
                    string checkFormat = DateTime.Now.ToString(dateFormat);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine(arguments[0] + ": format: not found");
                    return;
                }
                catch (FormatException)
                {
                    Console.WriteLine(arguments[0] + ": format: error value " + CommandLebedev.values[0]);
                    return;
                }
            }
            if (CommandLebedev.flagEnter.Contains("-s") || CommandLebedev.flagEnter.Contains("-f"))
                if (CommandLebedev.values.Count == 0)
                    Console.WriteLine(arguments[0] + ": error syntax");
                else if (CommandLebedev.values.Contains("next"))
                {
                    specifiedDay = DiscoverSpecifiedDay("next");
                    if (specifiedDay == 0)
                        return;
                    else Console.WriteLine(DateTime.Now.AddDays(currentDay >= specifiedDay ? 7 - (currentDay - specifiedDay) : specifiedDay - currentDay).ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US")));
                }
                else if (CommandLebedev.values.Contains("last"))
                {
                    specifiedDay = DiscoverSpecifiedDay("last");
                    if (specifiedDay == 0)
                        return;
                    else Console.WriteLine(DateTime.Now.AddDays(currentDay >= specifiedDay ? specifiedDay - currentDay == 0 ? -7 : specifiedDay - currentDay : specifiedDay - currentDay - 7).ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US")));
                }
                else if (CommandLebedev.values.Contains("yesterday"))
                    Console.WriteLine(DateTime.Now.AddDays(-1).ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US")));
                else if (CommandLebedev.values.Contains("tomorrow"))
                    Console.WriteLine(DateTime.Now.AddDays(1).ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US")));
                else if (CommandLebedev.flagEnter.Contains("-f"))
                    if (string.Join(" ", arguments).Split('"')[2] == "")
                        Console.WriteLine(DateTime.Now.ToString(dateFormat, CultureInfo.CreateSpecificCulture("en-US")));
                    else
                        Console.WriteLine("Invalid argument " + arguments[3]);
                else
                    Console.WriteLine("Invalid argument " + arguments[arguments.Length - 1]);
        }
        static int DiscoverSpecifiedDay(string op)
        {
            try
            {
                if (CommandLebedev.values.IndexOf("next") == -1)
                    index = CommandLebedev.values.IndexOf("last") + 1;
                else index = CommandLebedev.values.IndexOf("next") + 1;
                if (CommandLebedev.values[index].ToLower() == "monday" || CommandLebedev.values[index].ToLower() == "mon")
                    return 1;
                else if (CommandLebedev.values[index].ToLower() == "tuesday" || CommandLebedev.values[index].ToLower() == "tue")
                    return 2;
                else if (CommandLebedev.values[index].ToLower() == "wednesday" || CommandLebedev.values[index].ToLower() == "wed")
                    return 3;
                else if (CommandLebedev.values[index].ToLower() == "thursday" || CommandLebedev.values[index].ToLower() == "thu")
                    return 4;
                else if (CommandLebedev.values[index].ToLower() == "friday" || CommandLebedev.values[index].ToLower() == "fri")
                    return 5;
                else if (CommandLebedev.values[index].ToLower() == "saturday" || CommandLebedev.values[index].ToLower() == "sat")
                    return 6;
                else if (CommandLebedev.values[index].ToLower() == "sunday" || CommandLebedev.values[index].ToLower() == "sun")
                    return 7;
                else
                {
                    Console.WriteLine("Invalid argument " + CommandLebedev.values[index]);
                    return 0;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("date : " + op + ": error syntax");
                return 0;
            }
        }
    }
}
