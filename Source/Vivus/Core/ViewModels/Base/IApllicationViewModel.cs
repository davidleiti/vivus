namespace Vivus.Core.ViewModels.Base
{
    /// <summary>
    /// Represents an interface for a generic application viewmodel.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApllicationViewModel<T>
        where T : new()
    {
        /// <summary>
        /// Gets or sets the current logged in user.
        /// </summary>
        T User { get; set; }
    }
}
