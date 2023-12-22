using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TerminalLinux
{
    public static class Hexdump
    {
        public static void HexDump(string[] command)
        {
            Command.SplitCommand(command);
            Command.flag.Add("-C");
            Command.flag.Add("-d");
            Command.flag.Add("-b");
            Command.flag.Add("-h");
            Command.flag.Add("--help");
            if (!Command.CheckArguments())
            {
                Console.WriteLine("invalid flag");
                return;
            }
            if (Command.args.Contains("--help") || Command.args.Contains("-h"))
            {
                if (Command.args.Count == 1)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\hexdump.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            foreach (string value in Command.values)
            {
                Console.WriteLine("File name:" + value);
                if (Command.args.Contains("-b"))
                    EightInByte(value);
                else if (Command.args.Contains("-d"))
                    TenPerformanceByte(value);
                else
                    ConvertFile(value);
            }
        }
        static private void EightInByte(string fileName)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open))
                {
                    var buffer = new byte[16];
                    int bytesRead;

                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (var i = 0; i < bytesRead; i++)
                        {
                            Console.Write(Convert.ToString(buffer[i], 8) + " ");
                        }
                        if (Command.args.Contains("-C"))
                        {
                            Console.Write("|");
                            for (int i = 0; i < bytesRead; i++)
                            {
                                char c = (char)buffer[i];
                                if (char.IsControl(c)) c = '.';
                                Console.Write(c);
                            }
                            Console.WriteLine('|');
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
        static private void TenPerformanceByte(string fileName)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open))
                {
                    var buffer = new byte[16];
                    int bytesRead;

                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        for (var i = 0; i < bytesRead - 1; i++)
                        {
                            Console.Write(buffer[i] * 256 + buffer[i + 1] + " ");
                        }
                        if (Command.args.Contains("-C"))
                        {
                            Console.Write("|");
                            for (int i = 0; i < bytesRead; i++)
                            {
                                char c = (char)buffer[i];
                                if (char.IsControl(c)) c = '.';
                                Console.Write(c);
                            }
                            Console.WriteLine('|');
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
        static private void ConvertFile(string fileName)
        {
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    byte[] buffer = new byte[16];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        Console.Write("{0:X8}  ", stream.Position - bytesRead);

                        for (int i = 0; i < bytesRead; i++)
                        {
                            Console.Write("{0:X2} ", buffer[i]);
                            if (i == 7) Console.Write(" ");
                        }

                        if (bytesRead < 16) Console.Write(new string(' ', (16 - bytesRead) * 3));
                        Console.Write(" ");
                        if (Command.args.Contains("-C"))
                        {
                            Console.Write("|");
                            for (int i = 0; i < bytesRead; i++)
                            {
                                char c = (char)buffer[i];
                                if (char.IsControl(c)) c = '.';
                                Console.Write(c);
                            }
                            Console.WriteLine('|');
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}
