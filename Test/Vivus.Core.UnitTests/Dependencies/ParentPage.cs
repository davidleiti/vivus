namespace Vivus.Core.UnitTests.Dependencies
{
    using System.Security;
    using Vivus.Core.DataModels;

    public class ParentPage : IPage, IContainPassword
    {
        public SecureString SecurePasword => new SecureString();

        public void AllowErrors()
        {
        }

        public void AllowOptionalErrors()
        {
        }

        public void DontAllowErrors()
        {
        }
    }
}
