namespace Vivus.Core.Donor.Validators
{
    using System.Collections.Generic;
    using System.Security;

    /// <summary>
    /// Represents a validator for the donor application.
    /// </summary>
    public static class DonorValidator
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

        /// <summary>
        /// Validates the weight of the donor.
        /// </summary>
        /// <param name="weight">The weight of the donor.</param>
        /// <returns></returns>
        public static List<string> WeightValidation(int? weight)
        {
            if (!weight.HasValue)
                return new List<string> { "Weight field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the heart rate of the donor.
        /// </summary>
        /// <param name="hearRate">The heart rate of the donor.</param>
        /// <returns></returns>
        public static List<string> HearRateValidation(int? hearRate)
        {
            if (!hearRate.HasValue)
                return new List<string> { "Heart rate field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the systolic blood pressure of the donor.
        /// </summary>
        /// <param name="systolicBP">The systolic blood pressure of the donor.</param>
        /// <returns></returns>
        public static List<string> SystolicBloodPressureValidation(int? systolicBP)
        {
            if (!systolicBP.HasValue)
                return new List<string> { "Systolic blood pressure field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the diastolic blood pressure of the donor.
        /// </summary>
        /// <param name="diastolicBP">The diastolic blood pressure of the donor.</param>
        /// <returns></returns>
        public static List<string> DiastolicBloodPressureValidation(int? diastolicBP)
        {
            if (!diastolicBP.HasValue)
                return new List<string> { "Diastolic blood pressure field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the past surgeries of the donor.
        /// </summary>
        /// <param name="pastSurgeries">The past surgeries of the donor.</param>
        /// <returns></returns>
        public static List<string> PastSurgeriesValidation(string pastSurgeries)
        {
            if (!string.IsNullOrEmpty(pastSurgeries) && pastSurgeries.Length < 5)
                return new List<string> { "Please be more specific." };

            return null;
        }

        /// <summary>
        /// Validates the travelling status of the donor.
        /// </summary>
        /// <param name="travelStatus">The travelling status of the donor.</param>
        /// <returns></returns>
        public static List<string> TravelStatusValidation(string travelStatus)
        {
            if (!string.IsNullOrEmpty(travelStatus) && travelStatus.Length < 5)
                return new List<string> { "Please be more specific." };

            return null;
        }
    }
}
