using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;

namespace SupportBank
{
    class ReadFiles
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static List<Transaction> ImportFile(string filename)
        {
            logger.Debug("Trying to import data from "+ filename);
            if (Path.GetExtension(filename) == ".json")
            {
                return LoadJson(filename);
            }
            else
            {
                return ReadCsv(filename);
            }
        }
        public static List<Transaction> ReadCsv(string filename)
        {
            logger.Info("Started reading files.");
            string[] lines = System.IO.File.ReadAllLines(@filename);
            List<Transaction> output = MakeListTransactions(lines);
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
        
        public static List<Transaction> LoadJson(string filename)
        {
            using (StreamReader r = new StreamReader(@filename))
            {
                string json = r.ReadToEnd();
                List<Transaction> items = JsonConvert.DeserializeObject<List<Transaction>>(json);
                return items;
            }
        }
        
        public static List<Transaction> ImportTestData()
        {
            List<Transaction> fromCsv1 = ReadFiles.ImportFile(@"C:\Work\Training\Transactions2014.txt");
            List<Transaction> fromCsv2 = ReadFiles.ImportFile(@"C:\Work\Training\DodgyTransactions2015.txt");
            List<Transaction> fromJson = ReadFiles.ImportFile(@"C:\Work\Training\Transactions2013.json");
            List<Transaction> lines = fromCsv1.Concat(fromCsv2).ToList();
            List<Transaction> output = lines.Concat(fromJson).ToList();
            return output;
        }
    }
}