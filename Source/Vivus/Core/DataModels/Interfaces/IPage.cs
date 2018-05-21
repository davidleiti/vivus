namespace Vivus.Core.DataModels
{
    /// <summary>
    /// Represents an interface for a page.
    /// </summary>
    public interface IPage
    {
        /// <summary>
        /// Allows all the errors to be displayed.
        /// </summary>
        void AllowErrors();

        /// <summary>
        /// Resets the errors allow status.
        /// </summary>
        void DontAllowErrors();

        /// <summary>
        /// Allows only the optional errors to be displayed.
        /// </summary>
        void AllowOptionalErrors();
    }
}
