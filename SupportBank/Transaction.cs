using System.Collections.Generic;

namespace SupportBank
{
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