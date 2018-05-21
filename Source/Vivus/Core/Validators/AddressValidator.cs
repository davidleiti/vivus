namespace Vivus.Core.Validators
{
    using System.Collections.Generic;
    using Vivus.Core.DataModels;

    /// <summary>
    /// Represents a validator for the address view model.
    /// </summary>
    public static class AddressValidator
    {
        /// <summary>
        /// Validates the street name of the address.
        /// </summary>
        /// <param name="streetName">The street name of the address.</param>
        /// <returns></returns>
        public static List<string> StreetNameValidation(string streetName)
        {
            if (string.IsNullOrEmpty(streetName))
                return new List<string> { "Street name field is mandatory." };

            if (streetName.Length < 4)
                return new List<string> { "Street name cannot have less than 4 characters." };

            return null;
        }

        /// <summary>
        /// Validates the street number of the address.
        /// </summary>
        /// <param name="streetNumber">The street number of the address.</param>
        /// <returns></returns>
        public static List<string> StreetNumberValidation(string streetNumber)
        {
            if (string.IsNullOrEmpty(streetNumber))
                return new List<string> { "Street number field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the city of the address.
        /// </summary>
        /// <param name="city">The city of the address.</param>
        /// <returns></returns>
        public static List<string> CityValidation(string city)
        {
            if (string.IsNullOrEmpty(city))
                return new List<string> { "City field is mandatory." };

            if (city.Length < 4)
                return new List<string> { "City cannot have less than 4 characters." };

            return null;
        }

        /// <summary>
        /// Validates the county of the address.
        /// </summary>
        /// <param name="county">The county of the address.</param>
        /// <returns></returns>
        public static List<string> CountyValidation(BasicEntity<string> county)
        {
            if (county is null || county.Id < 0)
                return new List<string> { "County field is mandatory." };

            return null;
        }

        /// <summary>
        /// Validates the zip code of the address.
        /// </summary>
        /// <param name="zipCode">The zip code of the address.</param>
        /// <returns></returns>
        public static List<string> ZipCodeValidation(string zipCode)
        {
			if (!string.IsNullOrEmpty(zipCode) && zipCode.Length != 6)
                return new List<string> { "Zip code must have exactly 6 digits." };

            return null;
        }
    }
}
