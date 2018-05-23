namespace Vivus.Core.UnitTests.Dependencies
{
    using Vivus.Core.Security;

    public class Security : ISecurity
    {
        public string HashPassword(string password)
        {
            return password;
        }
    }
}
