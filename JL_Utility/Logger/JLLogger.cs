using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_Utility.Logger
{
    public class JLLogger : IJLLogger
    {
        public void Log(string message, JLLogType type = JLLogType.INFO)
        {
            string logMessage = getTypeAsString(type) + message;
            sendConsoleLog(logMessage, type);
        }

        private string getTypeAsString(JLLogType type)
        {
            switch (type)
            {
                case JLLogType.INFO:
                    return "[INFO] > ";
                case JLLogType.SUCCESS:
                    return "[SUCCESS] > ";
                case JLLogType.ERROR:
                    return "[ERROR] > ";
                case JLLogType.CRITICAL:
                    return "[CRITICAL] > ";
                case JLLogType.WARNING:
                    return "[WARNING] > ";
                default:
                    return "> ";
            }
        }

        private void sendConsoleLog(string message, JLLogType type)
        {
            var defaultColor = Console.BackgroundColor;
            switch (type)
            {
                case JLLogType.INFO:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case JLLogType.SUCCESS:
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case JLLogType.ERROR:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case JLLogType.CRITICAL:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case JLLogType.WARNING:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            Console.WriteLine(message);
            Console.BackgroundColor = defaultColor;
        }
    }
}
