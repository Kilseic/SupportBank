using System.Collections.Generic;

namespace SupportBank
{
    class Payments
    {
        public static Dictionary<string,Account> MakeAllPayments(string[] input, Dictionary<string,Account> accounts)
        {
            for (int i = 1; i < input.Length-1; i++)
            {
                string[] line = input[i].Split(',');
                accounts = MakePayment(accounts, line[1], line[2], line[4]);
                accounts = LogPayment(accounts,line[1], line[2], line);
            }
            return accounts;
        }

        private static string CreateTransaction(string[] line)
        {
            return (line[4] +". Date: " + line[0] + ". Reference: " +line[3]);
        }

        private static Dictionary<string, Account> LogPayment(Dictionary<string, Account> accounts,string person1, string person2, string[] line)
        {
            string transaction = CreateTransaction(line);
            accounts[person1].transactionHistory.Add("-£" +transaction);
            accounts[person2].transactionHistory.Add("+£" +transaction);
            return accounts;
        }
        private static Dictionary<string,Account> MakePayment(Dictionary<string,Account> accounts, string payer, string payee, string amount)
        {
            double amountD = double.Parse(amount);
            accounts = CheckAdded(accounts, payer);
            accounts = CheckAdded(accounts, payee);
            accounts[payer].amount -= amountD;
            accounts[payee].amount += amountD;
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
                List<string> temp2 = new List<string>();
                accounts[name].transactionHistory = temp2;
            }

            return accounts;
        }
    }
}