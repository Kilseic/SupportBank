﻿using System;
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
            StartLogging();
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            string[] lines = ReadFiles();
            try
            {
                accounts = Payments.MakeAllPayments(lines, accounts);
                while (true)
                {
                    string command = UserInput("Please enter a command: (or Exit)");
                    if (command == "Exit")
                    {
                        break;
                    }
                    Command.ExecuteCommand(command, accounts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Program stopped, invalid data imported.");
            }
        }

        private static void StartLogging()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }

        private static string UserInput(string question)
        {
            logger.Info("Asking for input.");
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        private static string[] ReadFiles()
        {
            logger.Info("Started reading files.");
            string[] lines = System.IO.File.ReadAllLines(@"C:\Work\Training\Transactions2014.txt");
            string[] lines2 = System.IO.File.ReadAllLines(@"C:\Work\Training\DodgyTransactions2015.txt");
            string[] allTransactions = new string[lines.Length + lines2.Length];
            lines.CopyTo(allTransactions, 0);
            lines2.CopyTo(allTransactions, lines.Length);
            return allTransactions;
        }
    }
}