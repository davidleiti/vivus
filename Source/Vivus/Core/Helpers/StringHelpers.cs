using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Transforms a phone number to a generic pattern.
        /// </summary>
        /// <param name="phoneNo">The phone number to transform.</param>
        /// <returns></returns>
        public static string FormatPhoneNumber(this string phoneNo)
        {
            Regex pattern;
            string newPhoneNo;

            pattern = new Regex(@"[\+\(\) ]");
            newPhoneNo = pattern.Replace(phoneNo, string.Empty);

            if (!string.IsNullOrEmpty(newPhoneNo))
            {
                if ("407".IndexOf(newPhoneNo[0]) < 0)
                    newPhoneNo = '7' + newPhoneNo;

                if (newPhoneNo[0] == '7')
                    newPhoneNo = '0' + newPhoneNo;

                if (newPhoneNo[0] == '0')
                    newPhoneNo = '4' + newPhoneNo;
            }

            return newPhoneNo;
        }
    }
}
