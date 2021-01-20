using System;
using System.IO;
using CleaningRobot.Json;

namespace CleaningRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                PrintHelp();
                Environment.Exit(1);
            }

            string inputJson = null;
            try
            {
                inputJson = File.ReadAllText(args[0]);
            }
            catch (Exception ex)
            {
                PrintError("Cannot read from source file:", ex);
                Environment.Exit(1);
            }

            string resultJson = null;
            try
            {
                var controller = new JsonController();
                resultJson = controller.Process(inputJson);
            }
            catch (Exception ex)
            {
                PrintError("Unexpected error during executing cleaning robot:", ex);
                Environment.Exit(1);
            }

            try
            {
                File.WriteAllText(args[1], resultJson);
            }
            catch (Exception ex)
            {
                PrintError("Cannot write to result file:", ex);
                Environment.Exit(1);
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Improper arguments.");
            Console.WriteLine("Usage:");
            Console.WriteLine("cleaning_robot.exe source.json result.json");
        }

        private static void PrintError(string message, Exception ex)
        {
            Console.WriteLine(message);
            Console.WriteLine(ex.Message);
        }
    }
}
