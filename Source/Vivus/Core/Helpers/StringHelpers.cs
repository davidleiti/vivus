namespace Vivus.Core.Helpers
{
    /// <summary>
    /// Represents a collection of methods to facilitate the work with string.
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Checks whether the current string contains one of the specified patterns.
        /// </summary>
        /// <param name="str">The current string.</param>
        /// <param name="patterns">The collection of patterns.</param>
        /// <returns></returns>
        public static bool Contains(this string str, string[] patterns)
        {
            foreach (string pattern in patterns)
                if (str.ToLower().Contains(pattern.ToLower()))
                    return true;

            return false;
        }
    }
}
