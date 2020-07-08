using System;
using NLog;

namespace SupportBank
{
    public class UserInteraction
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static void Start()
        {
            Command bank = new Command();
            string wantData = UserInput("Do you want to import the test data? Y/N");
            if (wantData == "Y")
            {
                bank.ImportTestData();
            }
            while (true)
            {
                string command = UserInput("Please enter a command: (or Exit)");
                if (command == "Exit")
                {
                    break;
                }
                try
                {
                    bank.ExecuteCommand(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error executing command, refer to log files for details.");
                    logger.Debug("Error executing command. " + ex.Message);
                }
            }
        }
        public static string UserInput(string question)
        {
            logger.Info("Asking for input.");
            Console.WriteLine(question);
            return Console.ReadLine();
        }
    }
}