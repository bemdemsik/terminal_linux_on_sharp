using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TerminalLinux
{
    static class Factor
    {
        //не рабочее
        public static void PrintResult(string[] args)
        {
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine(File.ReadAllText(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + "\\man\\factor.txt"));
                return;
            }
            if (args.Length < 2)
            {
                Console.WriteLine("An error occurred while executing the command. Not enough arguments.");
                return;
            }
            int[] numbers = GetNumbers(args);
            if(numbers.Length == 0)
            {
                return;
            }
            for(int i = 0; i<numbers.Length; i++)
            {
                FindSimpleMultiplie(numbers[i]);
            }
        }

        private static void FindSimpleMultiplie(int num)
        {
            Console.Write(num + ": ");
            for (int i = 0; num % 2 == 0; num /= 2)
            {
                Console.Write(" {0}", 2);
            }
            for (int i = 3; i <= num;)
            {
                if (num % i == 0)
                {
                    Console.Write(" {0}", i);
                    num /= i;
                }
                else
                {
                    i += 2;
                }
            }
            Console.WriteLine();
        }

        private static int[] GetNumbers(string[] args)
        {
            int[] numbers = new int[args.Length - 1];
            int j = 0;
            for(int z = 1; z<args.Length; z++)
            {
                try
                {
                    numbers[j] = Convert.ToInt32(args[z]);
                    j++;
                }
                catch(Exception)
                {
                    Console.WriteLine("An error occurred while executing the command. Check command syntax.");
                    return new int[0];
                }
            }
            int i = 0;
            foreach (string num in args)
            {
                try
                {
                    numbers[i] = Convert.ToInt32(num);
                    i++;
                }
                catch(Exception)
                {

                }
            }
            return numbers;
        }
    }
}
