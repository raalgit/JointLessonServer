using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_Migrator
{
    public static class Logger
    {
        public static void LogLine(string text, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.INFO:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LogLevel.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public enum LogLevel
    {
        INFO,
        SUCCESS,
        ERROR,
        WARNING
    }
}
