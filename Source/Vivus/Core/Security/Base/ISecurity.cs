namespace Vivus.Core.Security
{
    /// <summary>
    /// Represents a collection of security methods.
    /// </summary>
    public interface ISecurity
    {
        string HashPassword(string password);
    }
}
