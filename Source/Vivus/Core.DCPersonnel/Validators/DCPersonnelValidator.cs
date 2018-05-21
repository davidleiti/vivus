﻿using System.Collections.Generic;
using System.Security;
using Vivus.Core.DataModels;

namespace Vivus.Core.DCPersonnel.Validators
{
    /// <summary>
    /// Represents a validator for the donation center personnel application.
    /// </summary>
    public static class DCPersonnelValidator
    {
        /// <summary>
        /// Validates the email address of the donor.
        /// </summary>
        /// <param name="email">The email address of the donor.</param>
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
        /// Validates the password of the donor.
        /// </summary>
        /// <param name="password">The password of the donor.</param>
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
        public static List<string> MessagesValidation(string messages)
        {
            //   Vivus.Console.WriteLine(messages);
            if (string.IsNullOrEmpty(messages))
                return new List<string> { "Evaluation results field cannot be empty." };
            return null;
        }
        public static List<string> ContainerTypeValidation(BasicEntity<string> selectedContainer)
        {
            if (selectedContainer.Id < 0)
                return new List<string> { "Container type field is mandatory." };
            return null;
            //return new List<string> { "Container type is invalid." };
        }
        public static List<string> ContainerCodeValidation(BasicEntity<string> selectedContainer)
        {
            if (selectedContainer.Id < 0)
                return new List<string> { "Container code field is mandatory." };

            return null;
            //return new List<string> { "Container type is invalid." };
        }
        public static List<string> DonationCenterValidation(BasicEntity<string> donationCenter)
        {
            if (donationCenter==null||donationCenter.Id < 0)
            {
                return new List<string> { "Donation center field cannot be empty." };
            }
            return null;
        }
    }
}
