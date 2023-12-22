using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    static class Grep
    {
        static List<string> args = new List<string>();
        static public void GrepInFile(string[] command)
        {
            args = new List<string>();
            List<string> flag = new List<string>();
            flag.Add("-i");
            flag.Add("-n");
            flag.Add("-v");
            flag.Add("-h");
            flag.Add("--help");
            List<string> values = new List<string>();
            List<string> search = new List<string>();
            List<string> files = new List<string>();
            int i = 0;
            string text = "";
            bool check = false;
            if(command.Length==1)
            {
                Console.WriteLine("Invalid syntax");
                return;
            }
            foreach (string _args in command)
            {
                i++;
                if (i == 1)
                    continue;
                if (_args.ToCharArray()[0] == '-')
                {
                    args.Add(_args);
                    if (args.Contains("--help") || args.Contains("-h"))
                    {
                        if (args.Count == 1)
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\grep.txt"));
                        else
                            Console.WriteLine("Invalid key");
                        return;
                    }
                }
                else
                {
                    if (_args[0] == "'".ToCharArray()[0] || _args[0] == '"')
                    {
                        text += _args;
                        if (text[0] == text[text.Length - 1])
                        {
                            search.Add(text.Trim('"').Trim("'".ToCharArray()[0]));
                            continue;
                        }
                        else if (text[0] != '"' && text[0] == "'".ToCharArray()[0])
                            check = true;
                        else
                        {
                            Console.WriteLine("Invalid syntax");
                            return;
                        }
                    }
                    else if (_args[_args.Length - 1] == "'".ToCharArray()[0] || _args[_args.Length - 1] == '"')
                    {
                        if (text[0] == _args[_args.Length - 1])
                        {
                            text += " " + _args;
                            search.Add(text.Trim('"').Trim("'".ToCharArray()[0]));
                            text = "";
                            check = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid syntax");
                            return;
                        }
                    }
                    else if (check)
                        text += " " + _args;
                    else
                        values.Add(_args);
                }

            }
            foreach (string value in args)
            {
                if (!flag.Contains(value))
                {
                    Console.WriteLine("invalid Key");
                    return;
                }
            }
            foreach (string value in values)
            {
                string _value = value;
                if (value.Trim('"') != _value)
                {
                    search.Add(value.Trim('"'));
                }
                else if (value.Trim("'".ToCharArray()[0]) != _value)
                {
                    search.Add(value.Trim("'".ToCharArray()[0]));
                }
                else
                {
                    if (File.Exists(value))
                    {
                        files.Add(value);
                    }
                    else
                    {
                        search.Add(value);
                    }
                }
            }
            ReadFiles(files, search);
        }
        static private void ReadFiles(List<string> files, List<string> search)
        {
            foreach (string file in files)
            {
                try
                {
                    string output = "";
                    string outputWhisV = "";
                    using (StreamReader sr = new StreamReader(file))
                    {
                        int i = 1;
                        Console.WriteLine("File Name:" + file);
                        while (true)
                        {
                            string _str = sr.ReadLine();
                            if (_str == null)
                                break;
                            foreach (string value in search)
                            {
                                if (args.Contains("-i"))
                                    if (_str.ToLower().Contains(value.ToLower()))
                                        if (args.Contains("-n"))
                                            output += "Номер строки: " + i + "\t\t" + _str + "\n";
                                        else
                                            output += _str + "\n";
                                    else
                                        if (args.Contains("-n"))
                                        outputWhisV += "Номер строки: " + i + "\t\t" + _str + "\n";
                                    else
                                        outputWhisV += _str + "\n";
                                else
                                    if (_str.Contains(value))
                                        if (args.Contains("-n"))
                                            output += "Номер строки: " + i + "\t\t" + _str + "\n";
                                        else
                                            output += _str + "\n";
                                    else
                                            if (args.Contains("-n"))
                                        outputWhisV += "Номер строки: " + i + "\t\t" + _str + "\n";
                                    else
                                        outputWhisV += _str + "\n";

                            }
                            i++;
                        }
                    }
                    if (args.Contains("-v"))
                        if (outputWhisV == "")
                            Console.WriteLine("No matches found");
                        else
                            Console.WriteLine(outputWhisV);
                    else
                        if (output == "")
                        Console.WriteLine("No matches found");
                    else
                        Console.WriteLine(output);
                }
                catch (Exception)
                {
                    Console.WriteLine("File not found");
                }
            }
        }
    }
}
