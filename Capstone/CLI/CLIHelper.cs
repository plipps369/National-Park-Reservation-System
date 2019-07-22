using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Exceptions;

namespace Capstone.Helper
{
    class CLIHelper
    {
        /// <summary>
        /// Set this to true if you want exception messages and stack traces written to the console window
        /// </summary>
        public static bool EnableDebugInfo = false;

        /// <summary>
        /// Gets a valid date from the user using the console
        /// </summary>
        /// <param name="message">The message to display to the user requesting a date input</param>
        /// <returns>A populated datetime object with a valid date from the user</returns>
        public static DateTime GetDateTime(string message)
        {
            string userInput = String.Empty;
            DateTime dateValue = DateTime.MinValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid date format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!DateTime.TryParse(userInput, out dateValue));

            return dateValue;
        }

        /// <summary>
        /// Gets a valid integer from the user using the console
        /// </summary>
        /// <param name="message">The message to display to the user requesting an integer input</param>
        /// <returns>An integer from the user</returns>
        public static int GetInteger(string message)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;

        }

        /// <summary>
        /// Gets a valid integer from the user within a specified range using the console
        /// </summary>
        /// <param name="message">The message to display to the user requesting an integer input</param>
        /// <param name="startRange">The starting range for valid user input</param>
        /// <param name="endRange">The ending range for valid user input</param>
        /// <returns>An integer from the user within the specified range</returns>
        public static int GetInteger(string message, int min, int max)
        {
            int result = 0;
            int numberOfAttempts = 0;
            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                result = GetInteger(message);
                numberOfAttempts++;
            } while (result < min || result > max);
            return result;
        }

        /// <summary>
        /// Gets a valid single digit integer from the user within a specified range using the console
        /// </summary>
        /// <param name="message">The message to display to the user requesting an integer input</param>
        /// <param name="startRange">The starting range for valid user input</param>
        /// <param name="endRange">The ending range for valid user input</param>
        /// <returns>An integer from the user within the specified range</returns>
        public static int GetSingleInteger(string message, int startRange, int endRange)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            bool exit = false;
            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine($"\nInvalid input format. Selection must be a number between { startRange} and { endRange}.");
                }

                Console.Write(message + " ");
                userInput = Console.ReadKey().KeyChar.ToString();
                numberOfAttempts++;

                if (int.TryParse(userInput, out intValue))
                {
                    if (intValue >= startRange && intValue <= endRange)
                    {
                        exit = true;
                        Console.WriteLine();
                    }
                }
            }
            while (!exit);

            return intValue;

        }

        /// <summary>
        /// Gets a valid double from the user using the console
        /// </summary>
        /// <param name="message">The message to display to the user requesting a double input</param>
        /// <returns>A populated double variable from the user</returns>
        public static double GetDouble(string message)
        {
            string userInput = String.Empty;
            double doubleValue = 0.0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!double.TryParse(userInput, out doubleValue));

            return doubleValue;

        }

        /// <summary>
        /// Gets a valid bool from the user using the console
        /// </summary>
        /// <param name="message">The message to display to the user requesting a boolean input</param>
        /// <returns>A populated boolean variable from the user</returns>
        public static bool GetBool(string message)
        {
            string userInput = String.Empty;
            bool boolValue = false;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!bool.TryParse(userInput, out boolValue));

            return boolValue;
        }

        /// <summary>
        /// Gets a valid string from the user using the console
        /// </summary>
        /// <param name="message">The message to display to the user requesting a string input</param>
        /// <returns>A populated string object from the user</returns>
        public static string GetString(string message)
        {
            string userInput = String.Empty;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (String.IsNullOrEmpty(userInput));

            return userInput;
        }

        /// <summary>
        /// Writes the exception method and stack trace to the console
        /// </summary>
        /// <param name="e">The exception with the data to write to the console</param>
        public static void DisplayDebugInfo(Exception e)
        {
            if (EnableDebugInfo)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
            }
        }
    }
}

