using System;
using System.Collections.Generic;
using NLog;
using SupportBank;

namespace SupportBank
{
    public class Command
    {
        private Payments Bank = new Payments();
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public void ExecuteCommand(string command)
        {
            logger.Debug("Got this command from user: "+ command);
            if (command == "List All")
            {
                logger.Info("Listing all accounts and amounts.");
                foreach (KeyValuePair<string, Account> kvp in Bank.Accounts)
                {
                    Console.WriteLine(kvp.Key + ": £" + kvp.Value.Balance);
                }
            }
            else if (command.Substring(0, 5) == "List ")
            {
                string name = command.Substring(5, command.Length - 5);
                logger.Info("Listing all transactions for "+ name +".");
                Account value;
                if (Bank.Accounts.TryGetValue(name, out value))
                {
                    Console.WriteLine(name + ": £" + Bank.Accounts[name].Balance);
                    foreach (Transaction i in Bank.Accounts[name].TransactionHistory)
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
                }
                else
                {
                    Console.WriteLine("This person does not have an account.");
                }
            }
            else if (command.Substring(0,12) == "Import File ")
            {
                string fileName = command.Substring(12, command.Length - 12);
                List<Transaction> newPayments = ReadFiles.ImportFile(fileName);
                string answer = UserInteraction.UserInput("Would you like to import all valid data? Y/N");
                if (answer == "Y")
                {
                    Bank.MakeAllPayments(newPayments);
                    Console.WriteLine("Data Imported.");
                }
            }
            else
            {
                Console.WriteLine("Invalid command. ");
            }
        }
        public void ImportTestData()
        {
            List<List<Transaction>> files = new List<List<Transaction>>();
            files.Add(ReadFiles.ImportFile(@"C:\Work\Training\Transactions2014.txt"));
            files.Add(ReadFiles.ImportFile(@"C:\Work\Training\DodgyTransactions2015.txt"));
            files.Add(ReadFiles.ImportFile(@"C:\Work\Training\Transactions2013.json"));
            foreach (var file in files)
            {
                try
                {
                    Bank.MakeAllPayments(file);
                }
                catch
                {
                    logger.Debug("Error importing test data.");
                }
            }
        }
    }
}