namespace Vivus.Core.Helpers
{
    using Vivus.Core.Model;
    using Vivus.Core.ViewModels;

    /// <summary>
    /// Represents a collection of <see cref="Address"/> methods (helpers).
    /// </summary>
    public static class AddressHelpers
    {
        /// <summary>
        /// Returns a short format of the address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static string ShortAddress(this Address address)
        {
            return address.County.Name + ", " + address.City + ", " + address.Street + ", " + address.StreetNo;
        }

        /// <summary>
        /// Returns a short format of the address viewmodel.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static string ShortAddress(this AddressViewModel address)
        {
            return address.County + ", " + address.City + ", " + address.StreetName + ", " + address.StreetNumber;
        }
    }
}
