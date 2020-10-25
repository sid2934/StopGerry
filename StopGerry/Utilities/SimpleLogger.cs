using System;

namespace StopGerry.Utilities
{
    public class SimpleLogger
    {
        private const string FILE_EXT = ".log";
        private static readonly string datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static readonly string logFilename = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + FILE_EXT;

        private static bool _writeToConsole;

        /// <summary>
        /// Starts the SimpleLogger
        /// If log file does not exist, it will be created automatically.
        /// </summary>
        public static void Start(bool writeToConsole = false)
        {

            // Log file header line
            _writeToConsole = writeToConsole;
            WriteLine(System.DateTime.Now.ToString(datetimeFormat) + " Logging Started", false);
        }

        /// <summary>
        /// Stops the SimpleLogger
        /// If log file does not exist, it will be created automatically.
        /// </summary>
        public static void Stop()
        {

            // Log file header line
            Info("Logging Stopped");
        }

        /// <summary>
        /// Log a DEBUG message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Debug(string text)
        {
            WriteFormattedLog(LogLevel.DEBUG, text);
        }

        /// <summary>
        /// Log an ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Error(string text)
        {
            WriteFormattedLog(LogLevel.ERROR, text);
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Fatal(string text)
        {
            WriteFormattedLog(LogLevel.FATAL, text);
        }

        /// <summary>
        /// Log an INFO message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Info(string text)
        {
            WriteFormattedLog(LogLevel.INFO, text);
        }

        /// <summary>
        /// Log a TRACE message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Trace(string text)
        {
            WriteFormattedLog(LogLevel.TRACE, text);
        }

        /// <summary>
        /// Log a WARNING message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Warning(string text)
        {
            WriteFormattedLog(LogLevel.WARNING, text);
        }

        private static void WriteLine(string text, bool append = true)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logFilename, append, System.Text.Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
                if(_writeToConsole)
                {
                    Console.WriteLine(text);
                }
            }
            catch
            {
                throw;
            }
        }

        private static void WriteFormattedLog(LogLevel level, string text)
        {
            string pretext = level switch
            {
                LogLevel.TRACE => System.DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ",
                LogLevel.INFO => System.DateTime.Now.ToString(datetimeFormat) + " [INFO]    ",
                LogLevel.DEBUG => System.DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ",
                LogLevel.WARNING => System.DateTime.Now.ToString(datetimeFormat) + " [WARNING] ",
                LogLevel.ERROR => System.DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ",
                LogLevel.FATAL => System.DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ",
                _ => "",
            };
            WriteLine(pretext + text);
        }

        [System.Flags]
        private enum LogLevel
        {
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }
    }
}