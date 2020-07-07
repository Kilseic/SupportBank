namespace SupportBank
{
    public class Transaction
    {
        public string alpha;
        public string date;
        public string amount;
        public string to;
        public string from;
        public string reference;
        public override string ToString()
        {
            if (alpha == to)
            {
                return ("+£" + $"{amount:#0.00}" + ". Date: " + date + ". Reference: " + reference);
            }
            else
            {
                return ("-£" + $"{amount:#0.00}" +". Date: " + date + ". Reference: " +reference);
            }
        }
    }
}