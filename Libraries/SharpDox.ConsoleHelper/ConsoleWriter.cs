using System;
using System.Collections.Generic;

namespace SharpDox.ConsoleHelper
{
    public static class ConsoleWriter
    {
        public static void PrintConsoleHeader(string header)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(header);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
