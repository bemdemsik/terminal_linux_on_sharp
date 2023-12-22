using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace TerminalLinux
{
    class Mkpasswd
    {
        private static string passwd = string.Empty;
        private static string hashName = string.Empty;
        private static string salt = string.Empty;
        private static int[] codeSymbolsPwd = new int[40];
        private static int sumCode = 0;
        public static void GetHash(string[] arguments)
        {
            passwd = string.Empty;
            hashName = string.Empty;
            salt = string.Empty;
            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-h");
            CommandLebedev.flagCommand.Add("-m");
            CommandLebedev.flagCommand.Add("-S");
            if (!CommandLebedev.Fill(arguments))
                return;
            if (CommandLebedev.values.Count > 3 && CommandLebedev.flagEnter.Count == 2 || CommandLebedev.values.Count > 2 && CommandLebedev.flagEnter.Count == 1 || arguments.Length == 1)
            {
                Console.WriteLine("mkpasswd: value: error syntax");
                return;
            }

            if (CommandLebedev.flagEnter.Contains("-h"))
            {
                if (CommandLebedev.flagEnter.Count == 1 && CommandLebedev.values.Count == 0)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\mkpasswd.txt"));
                else
                    Console.WriteLine("Invalid key -h");
                return;
            }
            if (CommandLebedev.flagEnter.Contains("-S"))
                try
                {
                    if (!CommandLebedev.values.Contains(arguments[arguments.ToList().IndexOf("-S") + 1]))
                    {
                        Console.WriteLine("mkpasswd: value: -S: error syntax" + arguments[arguments.ToList().IndexOf("-S") + 1]);
                        return;
                    }
                    salt = arguments[arguments.ToList().IndexOf("-S") + 1];
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Invalid key -S value");
                    return;
                }
            if (CommandLebedev.flagEnter.Contains("-m"))
                try
                {
                    hashName = arguments[arguments.ToList().IndexOf("-m") + 1];
                    if (!CommandLebedev.values.Contains(arguments[arguments.ToList().IndexOf("-m") + 1]))
                    {
                        Console.WriteLine("mkpasswd: value: -m: error syntax" + arguments[arguments.ToList().IndexOf("-m") + 1]);
                        return;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Invalid value key -m");
                    return;
                }
            else hashName = "mynerealka";
            try
            {
                if (!CommandLebedev.flagEnter.Contains(arguments[1]))
                {
                    passwd = arguments[1];
                }
                else if (!CommandLebedev.flagEnter.Contains(arguments[3]))
                {
                    passwd = arguments[3];
                }
                else if (!CommandLebedev.flagEnter.Contains(arguments[5]))
                {
                    passwd = arguments[5];
                }
                else
                {
                    Console.WriteLine("mkpasswd: value: error syntax password");
                    return;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("mkpasswd: value: error syntax password");
                return;
            }
            switch (hashName.ToLower())
            {
                case "sha1":
                    if (salt != string.Empty)
                        GetSaltSHA1Hash();
                    else GetSHA1Hash();
                    break;
                case "sha256":
                    if (salt != string.Empty)
                        GetSaltSHA256Hash();
                    else GetSHA256Hash();
                    break;
                case "sha384":
                    if (salt != string.Empty)
                        GetSaltSHA384Hash();
                    else GetSHA384Hash();
                    break;
                case "sha512":
                    if (salt != string.Empty)
                        GetSaltSHA512Hash();
                    else GetSHA512Hash();
                    break;
                case "md5":
                    if (salt != string.Empty)
                        GetSaltMD5Hash();
                    else GetMD5Hash();
                    break;
                case "mynerealka":
                    GetMyNerealkaHash();
                    break;
                default:
                    Console.WriteLine("mkpasswd: not found algorithm " + hashName);
                    break;
            }
        }
        private static void GetSaltMD5Hash()
        {
            HMACMD5 md5 = new HMACMD5(Encoding.UTF8.GetBytes(salt));
            Console.WriteLine(md5.GetHashCode().ToString() + "$" + salt + "$" + Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetMD5Hash()
        {
            MD5 md5 = MD5.Create();
            Console.WriteLine(md5.GetHashCode().ToString() + "$" + Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSaltSHA1Hash()
        {
            HMACSHA1 sha1 = new HMACSHA1(Encoding.UTF8.GetBytes(salt));
            Console.WriteLine(sha1.GetHashCode().ToString() + "$" + salt + "$" + Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSHA1Hash()
        {
            SHA1 sha1 = SHA1.Create();
            Console.WriteLine(sha1.GetHashCode().ToString() + "$" + Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSaltSHA256Hash()
        {
            HMACSHA256 sha256 = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            Console.WriteLine(sha256.GetHashCode().ToString() + "$" + salt + "$" + Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSHA256Hash()
        {
            SHA256 sha256 = SHA256.Create();
            Console.WriteLine(sha256.GetHashCode().ToString() + "$" + Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSaltSHA384Hash()
        {
            HMACSHA384 sha384 = new HMACSHA384(Encoding.UTF8.GetBytes(salt));
            Console.WriteLine(sha384.GetHashCode().ToString() + "$" + salt + "$" + Convert.ToBase64String(sha384.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSHA384Hash()
        {
            SHA384 sha384 = SHA384.Create();
            Console.WriteLine(sha384.GetHashCode().ToString() + "$" + Convert.ToBase64String(sha384.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSaltSHA512Hash()
        {
            HMACSHA512 sha512 = new HMACSHA512(Encoding.UTF8.GetBytes(salt));
            Console.WriteLine(sha512.GetHashCode().ToString() + "$" + salt + "$" + Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetSHA512Hash()
        {
            SHA512 sha512 = SHA512.Create();
            Console.WriteLine(sha512.GetHashCode().ToString() + "$" + Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(passwd))));
        }
        private static void GetMyNerealkaHash()
        {
            if (salt != string.Empty)
                passwd += salt;
            string hash = string.Empty;
            codeSymbolsPwd = new int[40];
            int index = codeSymbolsPwd.Length - 1;
            sumCode = 0;
            for (int i = 0; i < passwd.Length; i++)
            {
                if (i >= 40)
                {
                    codeSymbolsPwd[index] += passwd[i];
                    index -= (passwd.Length % index + i % index);
                    if (index <= 0)
                        index += (codeSymbolsPwd.Length - 1);
                }
                else codeSymbolsPwd[i] = passwd[i];
                sumCode += passwd[i];
            }
            for (int i = 0; i < codeSymbolsPwd.Length; i++)
            {
                if (codeSymbolsPwd[i] == 0)
                    codeSymbolsPwd[i] = CodeSymbols(i + codeSymbolsPwd[sumCode % i] * sumCode);
                else codeSymbolsPwd[i] = CodeSymbols((i+1) * sumCode * codeSymbolsPwd[i]);
                hash += ((char)codeSymbolsPwd[i]).ToString();
                sumCode += codeSymbolsPwd[i];
            }
            Console.Write("11111$");
            if (salt != string.Empty)
                Console.Write(salt + "$");
            Console.WriteLine(hash);
        }
        private static int CodeSymbols(int number)
        {
            int code = number;
            while (!((code > 47 && code < 58) || (code > 64 && code < 91) || (code > 96 && code < 123) || code ==  61))
            {
                if (code >= 47)
                    code -= 67;
                else
                    code += 34;
            }
            return code;
        }
    }
}
