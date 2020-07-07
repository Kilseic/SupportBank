using System;
using System.Collections.Generic;

namespace SupportBank
{
    public static class Globals
    {
        public static Dictionary<string,double> accounts = new Dictionary<string, double>();
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            string[] lines = ReadFiles();
            MakeAllPayments(lines);
            string command = UserInput("Please enter a command.");
            ExecuteCommand(command);
        }

        private static string UserInput(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        private static void ExecuteCommand(string command)
        {
            if (command == "List All")
            {
                foreach (KeyValuePair<string,double> kvp in Globals.accounts)
                {
                    Console.WriteLine(kvp.Key + ": £" + kvp.Value);
                }
            } else if (command.Substring(0, 5) == "List ")
            {
                string name = command.Substring(5, command.Length - 5);
                double value;
                if (Globals.accounts.TryGetValue(name, out value))
                {
                    Console.WriteLine(name + ": £" + Globals.accounts[name]);
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

        private static void MakeAllPayments(string[] input)
        {
            for (int i = 1; i < input.Length-1; i++)
            {
                string[] line = input[i].Split(',');
                MakePayment(line[1], line[2], line[4]);
            }
        }

        private static string[] ReadFiles()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Work\Training\Transactions2014.txt");
            //string[] lines2 = System.IO.File.ReadAllLines(@"C:\Work\Training\DodgyTransactions2015.txt");
            //string[] allTransactions = new string[lines.Length + lines2.Length];
            //lines.CopyTo(allTransactions, 0);
            //lines2.CopyTo(allTransactions, lines.Length);
            //return allTransactions;
            return lines;
        }

        private static void MakePayment(string payer, string payee, string amount)
        {
            double amountD = double.Parse(amount);
            CheckAdded(payer);
            CheckAdded(payee);
            Globals.accounts[payer] -= amountD;
            Globals.accounts[payee] += amountD;
        }

        private static void CheckAdded(string name)
        {
            double value;
            if (!Globals.accounts.TryGetValue(name, out value))
            {
                Globals.accounts[name] = 0;
            }
        }
    }
}