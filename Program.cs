using System;
using System.Xml;

namespace MissingDigit
{
    public class Program
    {
        /// <summary>
        /// Find and return the missing digit in an algebraic equation represented as a string
        /// </summary>
        /// <param name="str">The equation that is being represented</param>
        /// <returns>The equation</returns>
        public static string MissingDigit(string str)
        {
            // Normalize the input by making sure there are spaces around the operations and equal sign to make splitting easier
            str = str.Replace("+", " + ").Replace("-", " - ").Replace("*", " * ").Replace("/", " / ").Replace("=", " = ");
            string[] parts = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string leftPart = "", middlePart = "", rightPart = "";
            char operation = ' ';
            bool foundOperation = false;

            // Parse through the equation into left, middle, and right components and identify the operation
            foreach (var part in parts)
            {
                if (part == "+" || part == "-" || part == "*" || part == "/")
                {
                    operation = part[0];
                    foundOperation = true;
                }
                else if (part == "=")
                {
                    foundOperation = false;
                }
                else if (!foundOperation && operation == ' ')
                {
                    leftPart = part;
                }
                else if (foundOperation)
                {
                    middlePart = part;
                }
                else
                {
                    rightPart = part;
                }
            }

            // Calculate and return the correct missing digit when 'x' is in the result part of the equation
            if (rightPart.Contains('x'))
            {
                // Handle 'x' in the result position
                double leftValue = double.Parse(leftPart);
                double middleValue = double.Parse(middlePart);
                double result = PerformOperation(leftValue, middleValue, operation);

                return result.ToString("0");
            }
            else
            {
                // Try all possible digits for 'x' in either the left or middle part of the equation
                for (int digit = 0; digit <= 9; digit++)
                {
                    string newLeftPart = leftPart.Contains('x') ? leftPart.Replace('x', (char)(digit + '0')) : leftPart;
                    string newMiddlePart = middlePart.Contains('x') ? middlePart.Replace('x', (char)(digit + '0')) : middlePart;
                    double leftValue = double.Parse(newLeftPart);
                    double middleValue = double.Parse(newMiddlePart);
                    double result = PerformOperation(leftValue, middleValue, operation);

                    if (Math.Abs(result - double.Parse(rightPart)) < 0.000001)
                    {
                        return digit.ToString();
                    }
                }
            }

            return str;

        }

        /// <summary>
        /// Helper function to perform the desired operation based on the operation character
        /// </summary>
        /// <param name="left">Left side of the equation</param>
        /// <param name="middle">Middle part of the equation</param>
        /// <param name="operation">The operation that is being performed</param>
        /// <returns>The end result of the operation that was used</returns>
        /// <exception cref="ArgumentException"></exception>
        private static double PerformOperation(double left, double middle, char operation)
        {
            switch (operation)
            {
                case '+':
                    return left + middle;
                case '-':
                    return left - middle;
                case '*':
                    return left * middle;
                case '/':
                    if (middle == 0)
                        throw new ArgumentException("Division by Zero.");
                    return left / middle;
            }
            throw new ArgumentException("Invalid operation.");
        }

        /// <summary>
        /// Gets the example equations from the array and then writes out the equation and what the missing digit is.
        /// </summary>
        static void Main()
        {
            string[] equations = { "4 - 2 = x", "1x0 + 12 = 112", "3x + 14 = 49", "4x5 + 5 = 500" };

            foreach (string equation in equations)
            {
                Console.WriteLine($"{equation} => Missing Digit: {MissingDigit(equation)}");
            }

            // keep this function call here *from original problem*
            //Console.WriteLine(MissingDigit(Console.ReadLine()));

        }
    }
}
