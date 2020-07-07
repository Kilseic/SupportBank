using System;
using System.Collections.Generic;
using NLog;
using SupportBank;

namespace SupportBank
{
    public class Command
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static void ExecuteCommand(string command, Dictionary<string, Account> accounts)
        {
            if (command == "List All")
            {
                logger.Info("Listing all accounts and amounts.");
                foreach (KeyValuePair<string, Account> kvp in accounts)
                {
                    Console.WriteLine(kvp.Key + ": £" + kvp.Value.amount);
                }
            }
            else if (command.Substring(0, 5) == "List ")
            {
                string name = command.Substring(5, command.Length - 5);
                logger.Info("Listing all transactions for "+ name +".");
                Account value;
                if (accounts.TryGetValue(name, out value))
                {
                    Console.WriteLine(name + ": £" + accounts[name].amount);
                    foreach (Transaction i in accounts[name].transactionHistory)
                    {
                        Console.WriteLine(i.ToString());
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
    }
}