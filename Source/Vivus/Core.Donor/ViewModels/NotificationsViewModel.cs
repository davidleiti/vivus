namespace Vivus.Core.Donor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Vivus.Core.DataModels;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.ViewModels.Notifications;

    /// <summary>
    /// Represents a view model for the notifications page.
    /// </summary>
    public class NotificationsViewModel : BaseViewModel
    {
        #region Private Members
        private IUnitOfWork unitOfWork;
        private IApllicationViewModel<Model.Donor> appViewModel;
        #endregion

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

            loadNotifications();

            // Test whether the binding was done right or not
            /*
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "AP", "andreipopescu", new DateTime(2018, 4, 28), "This is dă message."));
            });
            */
        }

        private async void loadNotifications()
        {
            await Task.Run(() =>
            {
                Model.Donor donor = unitOfWork.Persons[appViewModel.User.PersonID].Donor;
                List<Model.Message> messages = donor.Person.Messages.ToList();
                messages.Sort((m1, m2) =>
                {
                    if (m1.SendDate > m2.SendDate)
                        return -1;

                    if (m1.SendDate < m2.SendDate)
                        return 1;

                    return 0;
                });



                messages.ForEach(m =>
                {
                    char initial1 = m.Person.FirstName[0];
                    char initial2 = m.Person.LastName[0];
                    String initials = "" + initial1 + initial2;
                    dispatcherWrapper.InvokeAsync(() => Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), initials, m.Person.Donor.Account.Email, m.SendDate, m.Content)));
                });
            });
        }
        #endregion
    }
}
