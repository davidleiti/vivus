namespace Vivus.Core.Security
{
    using BCrypt.Net;

    /// <summary>
    /// Represents the implementation of a collection of security methods.
    /// </summary>
    public class Security : ISecurity
    {
        public string HashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }
    }
}
