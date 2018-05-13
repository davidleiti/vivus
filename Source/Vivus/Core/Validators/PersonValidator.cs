namespace Vivus.Core.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Represents a validator for the person view model.
    /// </summary>
    public static class PersonValidator
    {
        /// <summary>
        /// Validates the first name of the person.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns></returns>
        public static List<string> FirstNameValidation(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                return new List<string> { "First name field is mandatory." };

            if (firstName.Length < 2)
                return new List<string> { "First name cannot have less than 2 characters." };

            return null;
        }

        /// <summary>
        /// Validates the last name of the person.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns></returns>
        public static List<string> LastNameValidation(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                return new List<string> { "Last name field is mandatory." };

            if (lastName.Length < 2)
                return new List<string> { "Last name cannot have less than 2 characters." };

            return null;
        }

        /// <summary>
        /// Validates the birth date of the person.
        /// </summary>
        /// <param name="birthDate">The birth date of the person.</param>
        /// <returns></returns>
        public static List<string> BirthDateValidation(string birthDate)
        {
            if (string.IsNullOrEmpty(birthDate))
                return new List<string> { "Birth date field is mandatory." };

            if (!DateTime.TryParse(birthDate, new CultureInfo("ro-RO"), DateTimeStyles.AdjustToUniversal, out DateTime date))
                return new List<string> { "Birth date is not valid. Use dd/mm/yyyy format." };

            return null;
        }

        /// <summary>
        /// Validates the national identification number of the person.
        /// </summary>
        /// <param name="nin">The national identification number of the person.</param>
        /// <returns></returns>
        public static List<string> NinValidation(string nin)
        {
            if (string.IsNullOrEmpty(nin))
                return new List<string> { "National identification number field is mandatory." };

            if (nin.Length != 13)
                return new List<string> { "National identification number must have exactly 13 digits." };

            return null;
        }

        /// <summary>
        /// Validates the phone number of the person.
        /// </summary>
        /// <param name="phoneNo">The phone number of the person.</param>
        /// <returns></returns>
        public static List<string> PhoneNumberValidation(string phoneNo)
        {
            if (string.IsNullOrEmpty(phoneNo))
                return new List<string> { "Phone number field is mandatory." };

            if (phoneNo.Replace(" ", string.Empty).Length < 9)
                return new List<string> { "Phone number cannot have less than 9 characters." };

            if (phoneNo.Length > 18)
                return new List<string> { "Phone number cannot have more than 18 characters." };

            return null;
        }
    }
}
