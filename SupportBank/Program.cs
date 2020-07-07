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
            List<Transaction> fromCsv = ReadFiles.ReadCsv();
            List<Transaction> fromJson = ReadFiles.LoadJson();
            List<Transaction> lines = fromCsv.Concat(fromJson).ToList();
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