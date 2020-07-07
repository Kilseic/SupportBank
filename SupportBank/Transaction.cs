namespace SupportBank
{
    public class Transaction
    {
        public string alpha;
        public string Date;
        public string Amount;
        public string ToAccount;
        public string FromAccount;
        public string Narrative;
        public override string ToString()
        {
            if (alpha == ToAccount)
            {
                return ("+£" + $"{Amount:#0.00}" + ". Date: " + Date + ". Reference: " + Narrative);
            }
            else
            {
                return ("-£" + $"{Amount:#0.00}" +". Date: " + Date + ". Reference: " +Narrative);
            }
        }
    }
}