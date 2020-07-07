﻿using System;
using System.Collections.Generic;
using NLog;

namespace SupportBank
{
    class Payments
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static Dictionary<string,Account> MakeAllPayments(string[] input, Dictionary<string,Account> accounts)
        {
            for (int i = 1; i < input.Length-1; i++)
            {
                string[] line = input[i].Split(',');
                if (line[0] == "Date")
                {
                    continue;
                }
                try
                {
                    accounts = MakePayment(accounts, line[1], line[2], line[4]);
                    accounts = SavePayment(accounts,line[1], line[2], line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " Error occurred reading data on line " + (i+1) +".");
                    logger.Debug(ex, ex.Message + "This was on line " + (i+1));
                    //throw ex;
                }
            }
            return accounts;
        }

        private static Dictionary<string, Account> SavePayment(Dictionary<string, Account> accounts,string person1, 
            string person2, string[] line)
        {
            accounts = CheckAdded(accounts, person1);
            accounts = CheckAdded(accounts, person2);
            string[] names = {person1, person2};
            foreach (string i in names)
            {
                Transaction temp = new Transaction();
                temp.alpha = i;
                temp.date = line[0];
                temp.from = line[1];
                temp.to = line[2];
                temp.reference = line[3];
                temp.amount = line[4];
                accounts[i].transactionHistory.Add(temp);
            }
            return accounts;
        }
        private static Dictionary<string,Account> MakePayment(Dictionary<string,Account> accounts, string payer, 
            string payee, string amount)
        {
            double amountD = double.Parse(amount);
            accounts = CheckAdded(accounts, payer);
            accounts = CheckAdded(accounts, payee);
            accounts[payer].amount = Math.Round(accounts[payer].amount - amountD, 2);
            accounts[payee].amount = Math.Round(accounts[payee].amount + amountD, 2);
            return accounts;
        }

        private static Dictionary<string, Account> CheckAdded(Dictionary<string,Account> accounts, string name)
        {
            Account value;
            if (!accounts.TryGetValue(name, out value))
            {
                Account temp = new Account();
                accounts[name] = temp;
                accounts[name].amount = 0;
                List<Transaction> temp2 = new List<Transaction>();
                accounts[name].transactionHistory = temp2;
            }

            return accounts;
        }
    }
}