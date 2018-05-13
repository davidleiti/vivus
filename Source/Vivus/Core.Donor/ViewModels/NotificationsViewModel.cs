namespace Vivus.Core.Donor.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Notifications;

    /// <summary>
    /// Represents a view model for the notifications page.
    /// </summary>
    public class NotificationsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets the title of the notifications pannel.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the collection of the notifications.
        /// </summary>
        public ObservableCollection<NotificationViewModel> Items { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsViewModel"/> class with the default values.
        /// </summary>
        public NotificationsViewModel()
        {
            Title = "Latest notifications";
            Items = new ObservableCollection<NotificationViewModel>();

            // Test whether the binding was done right or not
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "AP", "andreipopescu", new DateTime(2018, 4, 28), "This is dă message."));
            });
        }

        #endregion
    }
}
