namespace Vivus.Core.DataModels
{
    using System.Security;

    /// <summary>
    /// Represents an interface for a class that can provide a secure password.
    /// </summary>
    public interface IContainPassword
    {
        /// <summary>
        /// The secure password.
        /// </summary>
        SecureString SecurePasword { get; }
    }
}
