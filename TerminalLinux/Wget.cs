using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Reflection;

namespace TerminalLinux
{
    public static class Wget
    {
        static string directory = string.Empty;
        static string file = string.Empty;
        static List<string> _arguments = new List<string>() { "-P", "-i", "-h", "-q" };
        static List<string> _files = new List<string>();
        static List<string> _userArguments = new List<string>();
        public static void Download(string[] commands)
        {
            if (commands.Length == 1)
            {
                Console.WriteLine("wget: the command takes values");
                return;
            }

            directory = string.Empty;
            file = string.Empty;
            _files.Clear();
            _userArguments.Clear();

            if (!CheckArguments(commands[0], commands))
            {
                return;
            }

            DownloadFiles(_files, _userArguments);

        }

        async public static void DownloadFiles(List<string> files, List<string> arguments)
        {
            List<string> links = new List<string>();
            bool onlyFiles = arguments.Contains("-i") ? true : false;

            if (onlyFiles && files.Count > 0)
            {
                Console.WriteLine("-i: argument takes a one file");
                return;
            }

            if (onlyFiles)
            {
                links = ReadFile(file);
                files.Clear();
                files = links;
            }

            string path = directory;

            if (path == string.Empty)
            {
                path = Directory.GetCurrentDirectory() + "\\";
            }

            try
            {
                await FileDownload(path, arguments, files);

            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong");
                return;
            }
        }

        async static public Task FileDownload(string directory, List<string> arguments, List<string> files)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(client_DownloadFileCompleted);

                await Task.WhenAll(files.Select(x =>
                {
                    string address = string.Empty;

                    string[] fullPath = x.Replace('/', '\\').Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < fullPath.Length - 1; i++)
                    {
                        address += fullPath[i] + "\\";
                    }

                    string fileName = fullPath[fullPath.Length - 1];

                    if (File.Exists(directory + "\\" + fileName))
                    {
                        File.Delete(directory + "\\" + fileName);
                    }

                    if (UrlExists(x))
                    {
                        if (!arguments.Contains("-q"))
                        {
                            Design(address, fileName, directory + "\\" + fileName);
                        }

                        return webClient.DownloadFileTaskAsync(new Uri(x), directory + "\\" + fileName);
                    }
                    else
                    {
                        Console.WriteLine("URL " + x + " not found");
                        return Task.FromResult<object>(null);
                    }
                }));
            }
        }

        static List<string> ReadFile(string file)
        {
            List<string> links = new List<string>();

            try
            {
                using (StreamReader streamReader = new StreamReader(file))
                {
                    string line = string.Empty;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        links.Add(line);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not work with the file");
            }

            return links;
        }

        static void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download completed!");
        }

        public static bool UrlExists(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public static void Design(string address, string fileName, string path)
        {
            Console.Write("Подключение к " + address + "...\nФайл " + fileName + "\n");
            Thread.Sleep(1500);
            Console.Write("соединение установлено.\n");

            Console.Write("HTTP-запрос отправлен. Ожидание ответа... ");
            Thread.Sleep(1500);
            Console.Write(" 200 OK\n");

            Console.WriteLine("Сохранение в <" + path.Replace(" ", "") + ">");
        }

        public static bool CheckArguments(string command, string[] userInput)
        {
            for (int i = 0; i < userInput.Length; i++)
            {
                bool check = false;
                if (i == 0)
                {
                    continue;
                }

                if (userInput[i].ToCharArray()[0] == '-')
                {
                    if (_arguments.Contains(userInput[i]))
                    {
                        if (userInput[i] == "-h")
                        {
                            Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\wget.txt"));
                            return false;
                        }

                        if (!_userArguments.Contains(userInput[i]))
                        {
                            try
                            {
                                if (userInput[i].ToCharArray()[1] == 'P')
                                {
                                    try
                                    {
                                        directory = userInput[i + 1];

                                        directory = CheckDirectory(directory);

                                        if (directory == string.Empty)
                                        {
                                            Console.WriteLine("-P: the directory must be exist");
                                            return false;
                                        }

                                        i++;

                                        _userArguments.Add(userInput[i - 1]);
                                        check = true;
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("-P: the directory must be specified");
                                        return false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Argument '-' does not exist");
                                return false;
                            }

                            try
                            {
                                if (userInput[i].ToCharArray()[1] == 'i')
                                {
                                    try
                                    {
                                        file = userInput[i + 1];

                                        file = CheckPath(file);

                                        if (file == string.Empty)
                                        {
                                            Console.WriteLine("-i: the file must be exist");
                                            return false;
                                        }

                                        i++;

                                        _userArguments.Add(userInput[i - 1]);
                                        check = true;
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("-P: the file must be specified");
                                        return false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Argument '-' does not exist");
                                return false;
                            }

                            if (!check)
                            {
                                _userArguments.Add(userInput[i]);
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid key " + userInput[i]);
                        return false;
                    }
                }
                else
                {
                    _files.Add(userInput[i]);
                }
            }

            return true;
        }

        public static string CheckPath(string file)
        {
            string path = string.Empty;

            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + file))
            {
                path = Directory.GetCurrentDirectory() + "\\" + file;
                return path;
            }
            else if (File.Exists(file))
            {

                path = file;
                return path;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string CheckDirectory(string path)
        {
            string _path = string.Empty;

            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + path))
            {
                _path = Directory.GetCurrentDirectory() + "\\" + path;
                return _path;
            }
            else if (Directory.Exists(path))
            {
                _path = path;
                return _path;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
