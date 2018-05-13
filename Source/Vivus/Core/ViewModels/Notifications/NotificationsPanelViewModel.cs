namespace Vivus.Core.ViewModels.Notifications
{
    using Vivus.Core.ViewModels;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a view model for a notifications panel.
    /// </summary>
    public class NotificationsPanelViewModel : BaseViewModel
    {
        #region Private Members

        private string title;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the title of the <see cref="NotificationsPanelViewModel"/>.
        /// </summary>
        public string Title
        {
            get => title;

            set
            {
                if (title == value)
                    return;

                title = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the side menu list items for the list.
        /// </summary>
        public ObservableCollection<NotificationViewModel> Items { get; set; }

        #endregion
    }
}
