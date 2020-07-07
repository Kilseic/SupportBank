using System;
using System.Collections.Generic;

namespace SupportBank
{
    public static class Globals
    {
        public static List<string> accountNames = new List<string>(); 
        public static List<double> accountValue = new List<double>();
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Work\Training\Transactions2014.txt");
            for (int i = 1; i < lines.Length-1; i++)
            {
                string[] line = lines[i].Split(',');
                makePayment(line[1], line[2], double.Parse(line[4]));
            }
            Console.WriteLine("Please enter a command.");
            string command = Console.ReadLine();
            if (command == "List All")
            {
                foreach (string name in Globals.accountNames)
                {
                    Console.WriteLine(name + ": £" + Globals.accountValue[Globals.accountNames.IndexOf(name)]);
                }
            } else if (command.Substring(0, 5) == "List ")
            {
                string name = command.Substring(5, command.Length - 5);
                if (Globals.accountNames.Contains(name))
                {
                    Console.WriteLine(name + ": £" + Globals.accountValue[Globals.accountNames.IndexOf(name)]);
                }
                else
                {
                    Console.WriteLine("This person does not have an account.");
                }
            }
            else
            {
                 Console.WriteLine("Invalid command");
            }
        }

        private static void makePayment(string payer, string payee, double amount)
        {
            checkAdded(payer);
            checkAdded(payee);
            Globals.accountValue[Globals.accountNames.IndexOf(payer)] -= amount;
            Globals.accountValue[Globals.accountNames.IndexOf(payee)] += amount;
        }

        private static void checkAdded(string name)
        {
            if (!Globals.accountNames.Contains(name))
            {
                Globals.accountNames.Add(name);
                Globals.accountValue.Add(0);
            }
        }
    }
}