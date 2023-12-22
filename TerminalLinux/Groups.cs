using System;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Reflection;

namespace TerminalLinux
{
    class Groups
    {
        static public void GroupsUsers(string[] arguments)
        {
            CommandLebedev.flagCommand.Clear();
            CommandLebedev.flagCommand.Add("-h");
            CommandLebedev.flagCommand.Add("-V");
            if (!CommandLebedev.Fill(arguments))
                return;

            else if (CommandLebedev.flagEnter.Contains("-V") || CommandLebedev.flagEnter.Contains("-h"))
            {
                if (CommandLebedev.values.Count > 0)
                    Console.WriteLine("groups: error value " + CommandLebedev.values[0]);
                else
                {
                    if (CommandLebedev.flagEnter.Contains("-V"))
                        Console.WriteLine("groups (GNU coreutils) 8.32");
                    if (CommandLebedev.flagEnter.Contains("-h"))
                        Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\groups.txt"));
                }
                return;
            }
            if (CommandLebedev.values.Count > 1)
            {
                Console.WriteLine("groups: error value " + CommandLebedev.values[1]);
            }
            else if (CommandLebedev.values.Count == 1)
                using (var context = new PrincipalContext(ContextType.Machine))
                {
                    var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, CommandLebedev.values[0]);
                    if (user != null)
                        Console.WriteLine("The user belongs to the following groups:\n" + string.Join("\n", user.GetGroups()));
                    else
                        Console.WriteLine("The user is not a member of any group");
                }
            else
            {
                DirectorySearcher Search = new DirectorySearcher(ContextType.Domain.ToString());
                Search.Filter = "(objectCategory=group)";
                SearchResultCollection ResultCollection = Search.FindAll();
                foreach (SearchResult result in ResultCollection)
                    Console.WriteLine(result.GetDirectoryEntry().Name.Remove(0, 3));
            }
        }
    }
}
