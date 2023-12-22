using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TerminalLinux
{
    class FileCommand
    {
        static public void TypeFile(string[] command)
        {
            Command.SplitCommand(command);
            Command.flag.Add("-b");
            Command.flag.Add("-f");
            Command.flag.Add("-k");
            Command.flag.Add("--help");
            Command.flag.Add("-h");
            if (!Command.CheckArguments())
            {
                Console.WriteLine("invalid flag");
                return;
            }
            if (Command.args.Contains("--help") || Command.args.Contains("-h"))
            {
                if (Command.args.Count == 1)
                    Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\file.txt"));
                else
                    Console.WriteLine("Invalid key");
                return;
            }
            foreach (string value in Command.values)
            {
                CheckFile(value);
            }
        }
        static private void CheckFile(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (Command.args.Contains("-b"))
                        GetFileType(fileInfo);
                    else
                    {
                        Console.Write("File Name: {0}\t", fileInfo.Name.Split('.')[0]);
                        GetFileType(fileInfo);
                    }
                }
                else
                {
                    if (Command.args.Contains("-f"))
                    {
                        List<string> str= new List<string>();
                        string[] path = file.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        if(path.Length==1)
                            str = Directory.GetFiles(Directory.GetCurrentDirectory(), file).ToList();
                        else
                            str = Directory.GetFiles(path[0], "*"+path[1]).ToList();
                        foreach(string _str in str)
                        {
                            FileInfo fileInfo = new FileInfo(_str);
                            if (Command.args.Contains("-b"))
                                GetFileType(fileInfo);
                            else
                            {
                                Console.Write("File Name: {0}\t", fileInfo.Name.Split('.')[0]);
                                GetFileType(fileInfo);
                            }
                        }
                    }
                    else
                        Console.WriteLine("File not found");
                }
                
            }
            catch (Exception)
            {
                Console.WriteLine("File not found");
                return;
            }
        }
        static void GetFileType(FileInfo fileInfo)
        {
            if (Command.args.Contains("-k"))
            {
                Console.Write("Size:" + fileInfo.Length.ToString()+",\t");
            }
            switch (fileInfo.Extension.ToLower())
            {
                case ".txt":
                    Console.WriteLine("File type: Text Document");
                    break;
                case ".doc":
                case ".docx":
                    Console.WriteLine("File type: Microsoft Word Document");
                    break;
                case ".xls":
                case ".xlsx":
                    Console.WriteLine("File type: Microsoft Excel Spreadsheet");
                    break;
                case ".ppt":
                case ".pptx":
                    Console.WriteLine("File type: Microsoft PowerPoint Presentation");
                    break;
                case ".pdf":
                    Console.WriteLine("File type: PDF File");
                    break;
                case ".jpg":
                case ".jpeg":
                    Console.WriteLine("File type: JPEG Image");
                    break;
                case ".png":
                    Console.WriteLine("File type: PNG Image");
                    break;
                case ".bmp":
                    Console.WriteLine("File type: BMP Image");
                    break;
                case ".gif":
                    Console.WriteLine("File type: GIF Image");
                    break;
                case ".avi":
                    Console.WriteLine("File type: AVI Video File");
                    break;
                case ".mp4":
                    Console.WriteLine("File type: MP4 Video File");
                    break;
                case ".mp3":
                    Console.WriteLine("File type: MP3 Audio File");
                    break;
                case ".wav":
                    Console.WriteLine("File type: WAV Audio File");
                    break;
                case ".xml": Console.WriteLine("This is an XML file."); break;
                case ".json": Console.WriteLine("This is a JSON file."); break;
                case ".css": Console.WriteLine("This is a CSS file."); break;
                case ".html": case ".htm": Console.WriteLine("This is an HTML file or webpage."); break;
                case ".js": Console.WriteLine("This is a JavaScript file."); break;
                case ".cs": case ".csproj": Console.WriteLine("This is a C# code file or project."); break;
                case ".java": Console.WriteLine("This is a Java file."); break;
                case ".py": Console.WriteLine("This is a Python file."); break;
                case ".rb": Console.WriteLine("This is a Ruby file."); break;
                case ".php": Console.WriteLine("This is a PHP file."); break;
                case ".cpp": case ".h": case ".hpp": case ".cc": case ".cxx": Console.WriteLine("This is a C++ code file."); break;
                case ".sql": Console.WriteLine("This is a SQL query file."); break;
                case ".psd": Console.WriteLine("This is an Adobe Photoshop file."); break;
                case ".ai": Console.WriteLine("This is an Adobe Illustrator file."); break;
                case ".eps": Console.WriteLine("This is an Encapsulated PostScript file."); break;
                case ".svg": Console.WriteLine("This is an SVG file."); break;
                case ".zip": case ".rar": Console.WriteLine("This is an archived file."); break;
                case ".dll": Console.WriteLine("This is a dynamic link library file."); break;
                default:
                    Console.WriteLine("Unrecognized file type: " + fileInfo.Extension.ToLower());
                    break;
            }
        }
    }
}
