using System;
using System.Collections.Generic;
using SupportBank;

namespace SupportBank
{
    public class Command
    {
        public static void ExecuteCommand(string command, string[] lines, Dictionary<string, Account> accounts)
        {
            if (command == "List All")
            {
                foreach (KeyValuePair<string, Account> kvp in accounts)
                {
                    Console.WriteLine(kvp.Key + ": £" + kvp.Value.amount);
                }
            }
            else if (command.Substring(0, 5) == "List ")
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
    }
}