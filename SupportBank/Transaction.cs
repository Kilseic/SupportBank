using System.Collections.Generic;

namespace SupportBank
{
    public class Account
    {
        public double amount;
        public List<Transaction> transactionHistory;
    }

    public class Transaction
    {
        public string Date;
        public string Amount;
        public string ToAccount;
        public string FromAccount;
        public string Narrative;
        public override string ToString()
        {
            return ($"{Amount:#0.00}" + ". Date: " + Date + ". Reference: " + Narrative);
        }
    }
}