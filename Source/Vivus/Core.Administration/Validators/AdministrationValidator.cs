using System.Collections.Generic;
using System.Security;
using Vivus.Core.DataModels;

namespace Vivus.Core.Administration.Validators
{
    /// <summary>
    /// Represents a validator for the administration application.
    /// </summary>
    public static class AdministrationValidator
    {
        /// <summary>
        /// Validates the email address of the DCPersonnel.
        /// </summary>
        /// <param name="email">The email address of the administrator.</param>
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
        /// Validates the password of the DCPersonnel.
        /// </summary>
        /// <param name="password">The password of the DCPersonnel.</param>
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
        /// Validates the selected donation center.
        /// </summary>
        /// <param name="donationCenter">The selected donation center.</param>
        /// <returns></returns>
        public static List<string> DonationCenterValidation(BasicEntity<string> donationCenter)
        {
            if (donationCenter is null || donationCenter.Id < 0)
                return new List<string> { "Donation center field is mandatory." };

            return null;
        }

    }
}
