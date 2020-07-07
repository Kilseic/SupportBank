using System;
using System.Collections.Generic;
using NLog;

namespace SupportBank
{
    class Payments
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static Dictionary<string,Account> MakeAllPayments(List<Transaction> input, Dictionary<string,
            Account> accounts)
        {
            for (int i = 1; i < input.Count; i++)
            {
                DateTime date;
                if (!DateTime.TryParse(input[i].Date, out date))
                {
                    logger.Info("Found invalid date on line " + (i+1));
                    continue;
                }
                try
                {
                    accounts = MakePayment(accounts, input[i].FromAccount, input[i].ToAccount,
                        input[i].Amount);
                    accounts = SavePayment(accounts,input[i].FromAccount, input[i].ToAccount, input[i]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " This was on line " + (i+1) +".");
                    logger.Debug(ex, ex.Message + "This was on line " + (i+1));
                    //throw ex;
                }
            }
            return accounts;
        }

        private static Dictionary<string, Account> SavePayment(Dictionary<string, Account> accounts,string person1, 
            string person2, Transaction line)
        {
            accounts = CheckAdded(accounts, person1);
            accounts = CheckAdded(accounts, person2);
            string[] names = {person1, person2};
            foreach (string i in names)
            {
                Transaction temp = line;
                temp.alpha = i;
                accounts[i].transactionHistory.Add(temp);
            }
            return accounts;
        }
        private static Dictionary<string,Account> MakePayment(Dictionary<string,Account> accounts, string payer, 
            string payee, string amount)
        {
            if (double.TryParse(amount, out double amountD))
            {
                accounts = CheckAdded(accounts, payer);
                accounts = CheckAdded(accounts, payee);
                accounts[payer].amount = Math.Round(accounts[payer].amount - amountD, 2);
                accounts[payee].amount = Math.Round(accounts[payee].amount + amountD, 2);
                return accounts;
            }
            throw new Exception("Cost not valid number. ");
        }

        private static Dictionary<string, Account> CheckAdded(Dictionary<string,Account> accounts, string name)
        {
            if (!accounts.TryGetValue(name, out Account value))
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