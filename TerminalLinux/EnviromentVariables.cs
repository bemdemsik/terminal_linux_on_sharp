using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Globalization;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class EnviromentVariables
    {
        static string username = string.Empty;
        static string home = string.Empty;
        static string path = string.Empty;
        static string hostname = string.Empty;
        static string lang = string.Empty;
        static string tz = string.Empty;
        static int histSize = 1000;

        public static void FillValiables()
        {
            username = Environment.UserName;
            home = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            hostname = Environment.MachineName;
            lang = "LC_" + CultureInfo.CurrentUICulture.ToString();
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            tz = localZone.DisplayName;
        }

        public static void GetVariables()
        {
            string allVars = string.Format("username={0}\nhome={1}\npath={2}\nhostname={3}\nlang={4}\ntz={5}", username, home, path, hostname, lang, tz);
            Console.WriteLine(allVars);
        }
        public static int HistSize
        {
            get
            {
                return histSize;
            }
            set
            {
                histSize = value;
            }
        }


    }
}
