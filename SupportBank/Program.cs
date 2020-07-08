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
    internal class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            StartLogging();
            Dictionary<string, Account> accounts = new Dictionary<string, Account>();
            string wantData = UserInput("Do you want to import the test data? Y/N");
            if (wantData == "Y")
            {
                List<Transaction> testDataTransactions = ReadFiles.ImportTestData();
                accounts = Payments.MakeAllPayments(testDataTransactions, accounts);
            }
            try
            {
                while (true)
                {
                    string command = UserInput("Please enter a command: (or Exit)");
                    if (command == "Exit")
                    {
                        break;
                    }
                    accounts = Command.ExecuteCommand(command, accounts);
                }
            }
            catch
            {
                Console.WriteLine("Program stopped, invalid data imported.");
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

        private static string UserInput(string question)
        {
            logger.Info("Asking for input.");
            Console.WriteLine(question);
            return Console.ReadLine();
        }
    }
}