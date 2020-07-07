using System;
using System.Collections.Generic;

namespace SupportBank
{
    class Account
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
            accounts = MakeAllPayments(lines, accounts);
            string command = UserInput("Please enter a command:");
            ExecuteCommand(command, lines, accounts);
        }

        private static string UserInput(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        private static void ExecuteCommand(string command, string[] lines, Dictionary<string,Account> accounts)
        {
            if (command == "List All")
            {
                foreach (KeyValuePair<string,Account> kvp in accounts)
                {
                    Console.WriteLine(kvp.Key + ": £" + kvp.Value.amount);
                }
            } else if (command.Substring(0, 5) == "List ")
            {
                string name = command.Substring(5, command.Length - 5);
                Account value;
                if (accounts.TryGetValue(name, out value))
                {
                    Console.WriteLine(name + ": £" + accounts[name].amount);
                    foreach (string i in accounts[name].transactionHistory)
                    {
                        Console.WriteLine(i);
                    }
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

        private static Dictionary<string,Account> MakeAllPayments(string[] input, Dictionary<string,Account> accounts)
        {
            for (int i = 1; i < input.Length-1; i++)
            {
                string[] line = input[i].Split(',');
                accounts = MakePayment(accounts, line[1], line[2], line[4]);
                accounts = LogPayment(accounts,line[1], line[2], line);
            }
            return accounts;
        }

        private static string CreateTransaction(string[] line)
        {
            return (line[4] +". Date: " + line[0] + ". Reference: " +line[3]);
        }

        private static Dictionary<string, Account> LogPayment(Dictionary<string, Account> accounts,string person1, string person2, string[] line)
        {
            string transaction = CreateTransaction(line);
            accounts[person1].transactionHistory.Add("-£" +transaction);
            accounts[person2].transactionHistory.Add("+£" +transaction);
            return accounts;
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

        private static Dictionary<string,Account> MakePayment(Dictionary<string,Account> accounts, string payer, string payee, string amount)
        {
            double amountD = double.Parse(amount);
            accounts = CheckAdded(accounts, payer);
            accounts = CheckAdded(accounts, payee);
            accounts[payer].amount -= amountD;
            accounts[payee].amount += amountD;
            return accounts;
        }

        private static Dictionary<string, Account> CheckAdded(Dictionary<string,Account> accounts, string name)
        {
            Account value;
            if (!accounts.TryGetValue(name, out value))
            {
                Account temp = new Account();
                accounts[name] = temp;
                accounts[name].amount = 0;
                List<string> temp2 = new List<string>();
                accounts[name].transactionHistory = temp2;
            }

            return accounts;
        }
    }
}