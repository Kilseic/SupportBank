using System;
using System.Collections.Generic;

namespace SupportBank
{
    public class Transaction
    {
        public DateTime Date;
        public double Amount;
        public string ToAccount;
        public string FromAccount;
        public string Narrative;
        public override string ToString()
        {
            return ($"{Amount:####0.00}" + ". Date: " + $"{Date:d}" + ". Reference: " + Narrative);
        }
    }
}