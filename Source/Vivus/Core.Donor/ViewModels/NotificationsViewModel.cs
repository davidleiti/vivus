namespace Vivus.Core.Donor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.Model;
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
        private int lastMessageId;
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
        public NotificationsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Title = "Latest notifications";
            Items = new ObservableCollection<NotificationViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Donor>>();
            lastMessageId = -1;

            UpdateNotifications();
        }

        private async void UpdateNotifications()
        {
            while (true)
            {
                await LoadNotifications();
                await Task.Delay(5000);
            }
        }

        private async Task LoadNotifications()
        {
            await Task.Run(() =>
            {
                Donor donor = unitOfWork.Persons[appViewModel.User.PersonID].Donor;
                List<Message> messages = new List<Message>();
                foreach(Message m in donor.Person.ReceivedMessages)
                {
                    if (m.MessageID > lastMessageId)
                        messages.Add(m);
                }
                messages.Sort((m1, m2) =>
                {
                    if (m1.SendDate > m2.SendDate)
                        return -1;

                    if (m1.SendDate < m2.SendDate)
                        return 1;

                    return 0;
                });

                if (messages.Count > 0 && messages[0].MessageID > lastMessageId)
                    lastMessageId = messages[0].MessageID;

                messages.ForEach(m =>
                {
                    char initial1 = m.Sender.FirstName[0];
                    char initial2 = m.Sender.LastName[0];
                    String initials = "" + initial1 + initial2;
                    dispatcherWrapper.InvokeAsync(() => Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), initials, FormatSenderName(m.Sender), m.SendDate, m.Content)));
                });

                string FormatSenderName(Person sender)
                {
                    return $"{ sender.FirstName.Replace("-", string.Empty).Replace(" ", string.Empty).ToLower() }.{ sender.LastName.Replace("-", string.Empty).Replace(" ", string.Empty).ToLower() }";
                }
            });
        }
        #endregion
    }
}
