namespace Vivus.Core.Doctor.Validators
{
    using System.Collections.Generic;
    using System.Security;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Represents a validator for the doctor application.
    /// </summary>
    public static class DoctorValidator
    {
        /// <summary>
        /// Validates the email address of the doctor.
        /// </summary>
        /// <param name="email">The email address of the doctor.</param>
        /// <returns></returns>
        public static List<string> EmailValidation(string email)
        {
            List<string> errors;

            if (string.IsNullOrEmpty(email))
                return new List<string> { "Email address field is mandatory." };

            if (email.Length > 32)
                return new List<string> { "Email address cannot have more than 32 characters." };

            errors = new List<string>();

            if (email.Length < 9)
                errors.Add("Email address cannot have less than 9 characters.");

            if (email.IndexOf("@") < 0)
                errors.Add("Email address must contain an [at] sign.");
            else
            {
                if (email.Substring(0, email.IndexOf("@")).Length < 3)
                    errors.Add("Email address (without domain) cannot have less than 3 characters.");

                if (email.Substring(email.IndexOf("@") + 1).IndexOf(".") < 0)
                    errors.Add("Email address's domain must contain a [dot] sign.");
                else
                {
                    if (email.Substring(email.IndexOf("@") + 1).IndexOf('.') == 0)
                        errors.Add("Email address's domain first character cannot be a [dot].");

                    if (email[email.Length - 1] == '.')
                        errors.Add("Email address's last character cannot be a [dot]");
                }
            }

            if (errors.Count > 0)
                return errors;

            return null;
        }

        /// <summary>
        /// Validates the password of the doctor.
        /// </summary>
        /// <param name="password">The password of the doctor.</param>
        /// <returns></returns>
        public static List<string> PasswordValidation(SecureString password)
        {
            if (password is null || password.Length == 0)
                return new List<string> { "Password field is mandatory." };

            if (password.Length < 8)
                return new List<string> { "Password cannot have less than 8 characters." };

            if (password.Length > 32)
                return new List<string> { "Password cannot have more than 32 characters." };

            return null;
        }

        /// <summary>
        /// Validates the selected person type.
        /// </summary>
        /// <param name="personType">The selected person type.</param>
        /// <returns></returns>
        public static List<string> PersonTypeValidation(BasicEntity<string> personType)
        {
            if (personType is null || personType.Id < 0)
                return new List<string> { "Person type field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the selected person name.
        /// </summary>
        /// <param name="personName">The selected person name.</param>
        /// <returns></returns>
        public static List<string> PersonNameValidation(BasicEntity<string> personName)
        {
            if (personName is null || personName.Id < 0)
                return new List<string> { "Person name field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the national identification number.
        /// </summary>
        /// <param name="nin">The national identification number.</param>
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
        /// Validates the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static List<string> MessageValidation(string message)
        {
            if (message is null)
                return new List<string> { "Message field must not be empty." };

            return null;
        }

    }
}
