using System;
using System.Collections.Generic;
using NLog;

namespace SupportBank
{
    public class Account
    {
        public double Balance;
        public List<Transaction> TransactionHistory;
    }
    public class Payments
    {
        public Dictionary<string, Account> Accounts = new Dictionary<string, Account>();
        
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public void MakeAllPayments(List<Transaction> input)
        {
            for (int i = 1; i < input.Count; i++)
            {
                DateTime date;
                if (!DateTime.TryParse(input[i].Date, out date))
                {
                    logger.Info("Found invalid date on line " + (i+1));
                    continue;
                }
                MakePayment(input[i].FromAccount, input[i].ToAccount,
                    input[i].Amount);
                SavePayment(input[i].FromAccount, input[i].ToAccount, input[i]);
            }
        }

        private void SavePayment(string person1, string person2, Transaction line)
        {
            CheckAdded(person1);
            CheckAdded(person2);
            string[] names = {person1, person2};
            foreach (string i in names)
            {
                Transaction temp = line;
                Accounts[i].TransactionHistory.Add(temp);
            }
        }
        
        private void MakePayment(string payer, string payee, string amount)
        {
            if (double.TryParse(amount, out double amountD))
            {
                CheckAdded(payer);
                CheckAdded(payee);
                Accounts[payer].Balance = Math.Round(Accounts[payer].Balance - amountD, 2);
                Accounts[payee].Balance = Math.Round(Accounts[payee].Balance + amountD, 2);
            }
        }

        private void CheckAdded(string name)
        {
            if (!Accounts.TryGetValue(name, out Account value))
            {
                Account temp = new Account();
                Accounts[name] = temp;
                Accounts[name].Balance = 0;
                List<Transaction> temp2 = new List<Transaction>();
                Accounts[name].TransactionHistory = temp2;
            }
        }
    }
}