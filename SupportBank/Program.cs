using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank
{
    interface IUserInteraction
    {
        Dictionary<string, Account> Accounts { get; set; }
    }
    internal class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            StartLogging();
            Payments accounts = new Payments();
            string wantData = UserInput("Do you want to import the test data? Y/N");
            if (wantData == "Y")
            {
                accounts = ImportTestData(accounts);
            }
            while (true)
            {
                string command = UserInput("Please enter a command: (or Exit)");
                if (command == "Exit")
                {
                    break;
                }
                try
                {
                    accounts = Command.ExecuteCommand(command, accounts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error executing command, refer to log files for details.");
                    logger.Debug("Error executing command. " + ex.Message);
                }
            }
        }

        private static void StartLogging()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log",
                Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }

        public static string UserInput(string question)
        {
            logger.Info("Asking for input.");
            Console.WriteLine(question);
            return Console.ReadLine();
        }
        
        public static Payments ImportTestData(Payments accounts)
        {
            List<List<Transaction>> files = new List<List<Transaction>>();
            files.Add(ReadFiles.ImportFile(@"C:\Work\Training\Transactions2014.txt"));
            files.Add(ReadFiles.ImportFile(@"C:\Work\Training\DodgyTransactions2015.txt"));
            files.Add(ReadFiles.ImportFile(@"C:\Work\Training\Transactions2013.json"));
            foreach (var file in files)
            {
                try
                {
                    accounts.MakeAllPayments(file);
                }
                catch
                {
                    logger.Debug("Error importing test data.");
                }
            }
            return accounts;
        }
    }
}