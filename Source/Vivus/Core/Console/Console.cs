using System;
using System.Diagnostics;

namespace Vivus.Core.Console
{
    /// <summary>
    /// Represents a collection of methods used in logging.
    /// </summary>
    public static class Console
    {
        /// <summary>
        /// Writes the specified string message, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="showDate">Wheter to write the date and time before the message or not.</param>
        /// <exception cref="System.IO.IOException"></exception>
        [Conditional("DEBUG")]
        public static void WriteLine(string message, bool showDate = true)
        {
            if (showDate)
            {
                string time = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                message = $"[{ time }] { message.Replace("\n", $"\n[{ time }] ").Replace($"{ Environment.NewLine }", $"\n[{ time }] ") }";
            }

            System.Console.WriteLine(message);
        }

        /// <summary>
        /// Writes the specified string message to the standard output stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="showDate">Wheter to write the date and time before the message or not.</param>
        [Conditional("DEBUG")]
        public static void Write(string message, bool showDate = true)
        {
            if (showDate)
            {
                string time = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                message = $"[{ time }] { message.Replace("\n", $"\n[{ time }] ").Replace($"{ Environment.NewLine }", $"\n[{ time }] ") }";
            }

            System.Console.Write(message);
        }

        /// <summary>
        /// Obtains the next character or function key pressed by the user. The pressed key is displayed in the console window.
        /// </summary>
        /// <returns></returns>
        public static ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }
    }
}
