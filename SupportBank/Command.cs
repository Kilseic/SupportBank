using System;
using System.Collections.Generic;
using NLog;
using SupportBank;

namespace SupportBank
{
    public class Command
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static Dictionary<string, Account> ExecuteCommand(string command, Dictionary<string, Account> accounts)
        {
            logger.Debug("Got this command from user: "+ command);
            if (command == "List All")
            {
                logger.Info("Listing all accounts and amounts.");
                foreach (KeyValuePair<string, Account> kvp in accounts)
                {
                    Console.WriteLine(kvp.Key + ": £" + kvp.Value.amount);
                }
                return accounts;
            }
            if (command.Substring(0, 5) == "List ")
            {
                string name = command.Substring(5, command.Length - 5);
                logger.Info("Listing all transactions for "+ name +".");
                Account value;
                if (accounts.TryGetValue(name, out value))
                {
                    Console.WriteLine(name + ": £" + accounts[name].amount);
                    foreach (Transaction i in accounts[name].transactionHistory)
                    {
                        if (name == i.FromAccount)
                        {
                            Console.WriteLine("-£" + i.ToString());
                        }
                        else
                        {
                            Console.WriteLine("+£" + i.ToString());
                        }
                    }
                    return accounts;
                }
                Console.WriteLine("This person does not have an account.");
                return accounts;
                
            }
            if (command.Substring(0,12) == "Import File ")
            {
                string fileName = command.Substring(12, command.Length - 12);
                List<Transaction> newPayments = ReadFiles.ImportFile(fileName);
                bool test = ReadFiles.CheckData(newPayments);
                if (test == false)
                {
                    string answer = Program.UserInput("Would you like to import all valid data? Y/N");
                    if (answer == "Y")
                    {
                        accounts = Payments.MakeAllPayments(newPayments, accounts);
                    }
                }
                Console.WriteLine("Data Imported.");
                return accounts;
            }
            Console.WriteLine("Invalid command");
            return accounts;
        }
    }
}