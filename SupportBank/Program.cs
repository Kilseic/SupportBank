using System;
using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank
{
    public class Account
    {
        public double amount;
        public List<Transaction> transactionHistory;
    }

    internal class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            startLogging();
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            string[] lines = ReadFiles();
            try
            {
                accounts = Payments.MakeAllPayments(lines, accounts);
                string command = UserInput("Please enter a command:");
                Command.ExecuteCommand(command, accounts);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program stopped, invalid data imported.");
            }
            
        }

        private static void startLogging()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }

        private static string UserInput(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        private static string[] ReadFiles()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Work\Training\Transactions2014.txt");
            string[] lines2 = System.IO.File.ReadAllLines(@"C:\Work\Training\DodgyTransactions2015.txt");
            string[] allTransactions = new string[lines.Length + lines2.Length];
            lines.CopyTo(allTransactions, 0);
            lines2.CopyTo(allTransactions, lines.Length);
            return allTransactions;
        }
    }
}