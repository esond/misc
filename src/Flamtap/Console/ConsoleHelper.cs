using System;

namespace Flamtap.Console
{
    public static class ConsoleHelper
    {
        public static void WriteSuccess(string message)
        {
            var oldFg = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = oldFg;
        }

        public static void WriteInfo(string message)
        {
            var oldFg = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = oldFg;
        }

        public static void WriteWarn(string message)
        {
            var oldFg = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = oldFg;
        }

        public static void WriteError(string message)
        {
            var oldFg = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = oldFg;
        }
    }
}
