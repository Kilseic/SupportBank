using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace SupportBank
{
    class ReadFiles
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static List<Transaction> ReadCsv()
        {
            logger.Info("Started reading files.");
            string[] lines = System.IO.File.ReadAllLines(@"C:\Work\Training\Transactions2014.txt");
            string[] lines2 = System.IO.File.ReadAllLines(@"C:\Work\Training\DodgyTransactions2015.txt");
            string[] allTransactions = new string[lines.Length + lines2.Length];
            lines.CopyTo(allTransactions, 0);
            lines2.CopyTo(allTransactions, lines.Length);
            List<Transaction> output = MakeListTransactions(allTransactions);
            return output;
        }

        private static List<Transaction> MakeListTransactions(string[] input)
        {
            List < Transaction > output = new List<Transaction>();
            for (int i = 1; i < input.Length; i++)
            {
                string[] line = input[i].Split(',');
                DateTime date;
                if (!DateTime.TryParse(line[0], out date))
                {
                    logger.Info("Found invalid date on line " + (i+1) + " in CSV");
                    continue;
                }
                Transaction temp = new Transaction();
                temp.Date = line[0];
                temp.FromAccount = line[1];
                temp.ToAccount = line[2];
                temp.Narrative = line[3];
                temp.Amount = line[4];
                output.Add(temp);
            }

            return output;
        }
        
        public static List<Transaction> LoadJson()
        {
            using (StreamReader r = new StreamReader(@"C:\Work\Training\Transactions2013.json"))
            {
                string json = r.ReadToEnd();
                List<Transaction> items = JsonConvert.DeserializeObject<List<Transaction>>(json);
                return items;
            }
        }
    }
}