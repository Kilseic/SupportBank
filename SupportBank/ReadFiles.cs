using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
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
            else if (Path.GetExtension(filename) == ".xml")
            {
                return LoadXml(filename);
            }
            else
            {
                return ReadCsv(filename);
            }
        }
        private static List<Transaction> ReadCsv(string filename)
        {
            logger.Info("Started reading files.");
            string[] lines = System.IO.File.ReadAllLines(@filename);
            List<Transaction> output = MakeListTransactions(lines);
            return output;
        }

        private static List<Transaction> LoadXml(string filename)
        {
            List<Transaction> output = new List<Transaction>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            foreach (XmlNode node in doc.DocumentElement)
            {
                Transaction temp = new Transaction();
                double dateCode = double.Parse(node.Attributes[0].InnerText);
                temp.Date = DateTime.FromOADate(dateCode);
                foreach (XmlNode child in node.ChildNodes)
                {
                    try
                    {
                        switch (child.Name)
                        {
                            case "Description":
                            {
                                temp.Narrative = child.InnerText;
                                break;
                            }
                            case "Value":
                            {
                                if (Double.TryParse(child.InnerText, out double amount))
                                {
                                    temp.Amount = amount;
                                }
                                else
                                {
                                    temp.Amount = 0;
                                }

                                break;
                            }
                            case "Parties":
                            {
                                foreach (XmlNode childDeep in child.ChildNodes)
                                {
                                    if (childDeep.Name == "To")
                                    {
                                        temp.ToAccount = childDeep.InnerText;
                                    }
                                    else
                                    {
                                        temp.FromAccount = childDeep.InnerText;
                                    }
                                }

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Debug(ex.Message + " Erorr in data found in Xml.");
                        Console.WriteLine("Invalid data found. ");
                    }
                }
                output.Add(temp);
            }
            return output;
        }

        private static List<Transaction> MakeListTransactions(string[] input)
        {
            List < Transaction > output = new List<Transaction>();
            for (int i = 1; i < input.Length; i++)
            {
                string[] line = input[i].Split(',');
                Transaction temp = new Transaction();
                try
                {
                    temp.Date = Convert.ToDateTime(line[0]);
                    temp.FromAccount = line[1];
                    temp.ToAccount = line[2];
                    temp.Narrative = line[3];
                    temp.Amount = Convert.ToDouble(line[4]);
                    output.Add(temp);
                }
                catch
                {
                    logger.Debug("Invalid date/amount found on line " + i+1);
                    Console.WriteLine("Invalid date/amount found on line " + i+1);
                }
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
    }
}