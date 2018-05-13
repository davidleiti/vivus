using System;

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
        public static void WriteLine(string message, bool showDate = true)
        {
            if (showDate)
                message = $"[{ DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") }] { message }";

            System.Console.WriteLine(message);
        }

        /// <summary>
        /// Writes the specified string message to the standard output stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="showDate">Wheter to write the date and time before the message or not.</param>
        public static void Write(string message, bool showDate = true)
        {
            if (showDate)
                message = $"[{ DateTime.Now }] { message }";

            System.Console.Write(message);
        }
    }
}
