using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            List<Transaction> fromCsv = ReadCsv();
            List<Transaction> fromJson = LoadJson();
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

        private static List<Transaction> ReadCsv()
        {
            logger.Info("Started reading files.");
            string[] lines = System.IO.File.ReadAllLines(@"C:\Work\Training\Transactions2014.txt");
            string[] lines2 = System.IO.File.ReadAllLines(@"C:\Work\Training\DodgyTransactions2015.txt");
            string[] allTransactions = new string[lines.Length + lines2.Length];
            lines.CopyTo(allTransactions, 0);
            lines2.CopyTo(allTransactions, lines.Length);
            List<Transaction> output = MakeListTransactions(allTransactions);
            return output;
        }

        private static List<Transaction> MakeListTransactions(string[] input)
        {
            List < Transaction > output = new List<Transaction>();
            for (int i = 1; i < input.Length; i++)
            {
                string[] line = input[i].Split(',');
                DateTime date;
                if (!DateTime.TryParse(line[0], out date))
                {
                    logger.Info("Found invalid date on line " + (i+1) + " in CSV");
                    continue;
                }
                Transaction temp = new Transaction();
                temp.Date = line[0];
                temp.FromAccount = line[1];
                temp.ToAccount = line[2];
                temp.Narrative = line[3];
                temp.Amount = line[4];
                output.Add(temp);
            }

            return output;
        }
        
        public static List<Transaction> LoadJson()
        {
            using (StreamReader r = new StreamReader(@"C:\Work\Training\Transactions2013.json"))
            {
                string json = r.ReadToEnd();
                List<Transaction> items = JsonConvert.DeserializeObject<List<Transaction>>(json);
                return items;
            }
        }
    }
}