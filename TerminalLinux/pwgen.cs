using System;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.IO;

namespace TerminalLinux
{
    class Pwgen
    {
        static int lenPassword = 0;
        static int countPasswords = 0;
        static StringBuilder sb = new StringBuilder();
        static string lunaticsSymbol = string.Empty;
        static string useSymbol = string.Empty;
        static string vowels = "aeyuioAEYUIO";
        static string number = "0123456789";
        static string symbols = "!@#$%^&*()-_=+;,<.>/?";
        static public void PasswordGeneration(string[] arguments)
        {
            lunaticsSymbol = string.Empty;
            useSymbol = "bcdfghjklmnpqrstvwxzBCDFGHJKLMNPQRSTVWXZ";
            lenPassword = 8;
            countPasswords = 120;
            Random rn = new Random();
            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-h");
            CommandLebedev.flagCommand.Add("-y");
            CommandLebedev.flagCommand.Add("-s");
            CommandLebedev.flagCommand.Add("-v");
            CommandLebedev.flagCommand.Add("-n");
            CommandLebedev.flagCommand.Add("-l");
            if (!CommandLebedev.Fill(arguments))
                return;
            if (CommandLebedev.values.Count > 2)
            {
                Console.WriteLine("pwgen: value: error syntax");
                return;
            }
            else if (!CommandLebedev.flagEnter.Contains("-n") && !CommandLebedev.flagEnter.Contains("-l") && CommandLebedev.values.Count > 0)
            {
                Console.WriteLine("pwgen: value: error syntax");
                return;
            }
            else if (!CommandLebedev.flagEnter.Contains("-n") && CommandLebedev.flagEnter.Contains("-l") && CommandLebedev.values.Count > 1 || CommandLebedev.flagEnter.Contains("-n") && !CommandLebedev.flagEnter.Contains("-l") && CommandLebedev.values.Count > 1)
            {
                Console.WriteLine("pwgen: value: error syntax");
                return;
            }

            if (CommandLebedev.flagEnter.Contains("-h"))
            {
                if (CommandLebedev.flagEnter.Count == 1)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\pwgen.txt"));
                else
                    Console.WriteLine("Invalid key -h");
                return;
            }

            if (CommandLebedev.flagEnter.Contains("-n"))
            {
                try
                {
                    if (!int.TryParse(arguments[arguments.ToList().IndexOf("-n") + 1], out countPasswords))
                    {
                        Console.WriteLine("pwgen: value: error count passwd");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("pwgen: value: error count passwd");
                    return;
                }
            }
            if (CommandLebedev.flagEnter.Contains("-l"))
            {
                try
                {
                    if (!int.TryParse(arguments[arguments.ToList().IndexOf("-l") + 1], out lenPassword))
                    {
                        Console.WriteLine("pwgen: value: error length passwd");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("pwgen: value: error length passwd");
                    return;
                }
            }
            if (CommandLebedev.flagEnter.Contains("-y"))
                useSymbol += symbols;
            if (!CommandLebedev.flagEnter.Contains("-v"))
                useSymbol += (vowels + number);
            char[] allUseSymbols = useSymbol.ToCharArray();
            for (int i = 0; i < useSymbol.Length - 1; i++)
            {
                int index = rn.Next(0, allUseSymbols.Length - 1);
                char symbol = allUseSymbols[index];
                allUseSymbols[index] = allUseSymbols[i];
                allUseSymbols[i] = symbol;
            }
            lunaticsSymbol = string.Concat(allUseSymbols);
            if (CommandLebedev.flagEnter.Contains("-s"))
                GenerateSecurityPassword();
            else GeneratePassword();
            return;
        }
        public static void GenerateSecurityPassword()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < countPasswords; i++)
                {
                    sb.Clear();
                    byte[] nonce = new byte[lenPassword];
                    rng.GetBytes(nonce);
                    while (sb.Length < lenPassword)
                        sb.Append(lunaticsSymbol[nonce[sb.Length] % lunaticsSymbol.Length]);
                    Console.Write((i + 1).ToString() + ": " + sb.ToString() + "\t");
                    if(i % 4 == 1)
                        Console.WriteLine();
                }
            }
            Console.WriteLine();
        }
        public static void GeneratePassword()
        {
            Random rn = new Random();
            for (int i = 0; i < countPasswords; i++)
            {
                sb.Clear();
                while (sb.Length < lenPassword)
                    sb.Append(lunaticsSymbol[rn.Next(0, lunaticsSymbol.Length - 1)]);
                Console.Write((i + 1).ToString() + ": " + sb.ToString() + "\t");
                if (i % 4 == 1)
                    Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
