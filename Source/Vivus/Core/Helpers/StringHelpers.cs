using System.Globalization;
using System.Text;

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
                if (str.ToLower().RemoveDiacritics().Contains(pattern.ToLower().RemoveDiacritics()))
                    return true;

            return false;
        }

        /// <summary>
        /// Checks whether the current string contains one of the specified patterns starting from the begining of the string.
        /// </summary>
        /// <param name="str">The current string.</param>
        /// <param name="patterns">The collection of patterns.</param>
        /// <returns></returns>
        public static bool ContainsFromBegining(this string str, string[] patterns)
        {
            foreach (string pattern in patterns)
                if (str.ToLower().RemoveDiacritics().IndexOf(pattern.ToLower().RemoveDiacritics()) == 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Removes the diacritics from a string.
        /// </summary>
        /// <param name="str">The string to remove the diacritics from.</param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string str)
        {
            StringBuilder stringBuilder;
            string formD;

            stringBuilder = new StringBuilder();
            formD = str.Normalize(NormalizationForm.FormD);

            foreach (char c in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
