namespace Vivus.Core.DataModels
{
    using Vivus.Core.ViewModels;
    using Windows.Theme.Data;

    /// <summary>
    /// Represents an interface for a popup window.
    /// </summary>
    public interface IPopup
    {
        #region Properties

        /// <summary>
        /// Gets or sets the object that owns that <see cref="IPopup"/>.
        /// </summary>
        IWindow Owner { get; set; }

        /// <summary>
        /// Gets or sets the current page on the popup.
        /// </summary>
        IPage CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the viewmodel of the current page.
        /// </summary>
        BaseViewModel PageViewModel { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a popup and returns without waiting for the newly opened popup to close.
        /// </summary>
        void Show();

        /// <summary>
        /// Opens a popup and returns only when the newly opened popup is closed.
        /// </summary>
        /// <param name="viewModel">The page viewmodel.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns></returns>
        bool? ShowDialog(BaseViewModel viewModel);

        #endregion
    }
}
