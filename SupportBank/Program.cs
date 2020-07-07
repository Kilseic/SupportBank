using System;
using System.Collections.Generic;

namespace SupportBank
{
    public class Account
    {
        public double amount;
        public List<string> transactionHistory;

    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            string[] lines = ReadFiles();
            accounts = Payments.MakeAllPayments(lines, accounts);
            string command = UserInput("Please enter a command:");
            Command.ExecuteCommand(command, lines, accounts);
        }

        private static string UserInput(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        private static string[] ReadFiles()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Work\Training\Transactions2014.txt");
            return lines;
            //string[] lines2 = System.IO.File.ReadAllLines(@"C:\Work\Training\DodgyTransactions2015.txt");
            //string[] allTransactions = new string[lines.Length + lines2.Length];
            //lines.CopyTo(allTransactions, 0);
            //lines2.CopyTo(allTransactions, lines.Length);
            //return allTransactions;
        }
    }
}