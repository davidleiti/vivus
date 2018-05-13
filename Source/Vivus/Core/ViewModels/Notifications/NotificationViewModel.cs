namespace Vivus.Core.ViewModels.Notifications
{
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using System;

    /// <summary>
    /// Represents a notification view model.
    /// </summary>
    public class NotificationViewModel : BaseViewModel
    {
        #region Private Members

        private ArgbColor profileBrush;
        private string nameInitials;
        private string username;
        private DateTime sentDate;
        private string message;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the background brush for the profile.
        /// </summary>
        public ArgbColor ProfileBrush
        {
            get => profileBrush;

            set
            {
                if (profileBrush == value)
                    return;

                profileBrush = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the profile initials.
        /// </summary>
        public string NameInitials
        {
            get => nameInitials;

            set
            {
                if (nameInitials == value)
                    return;

                nameInitials = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the username of the sender.
        /// </summary>
        public string Username
        {
            get => username;

            set
            {
                if (username == value)
                    return;

                username = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the send date of the notification.
        /// </summary>
        public DateTime SentDate
        {
            get => sentDate;

            set
            {
                if (sentDate == value)
                    return;

                sentDate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the message of the notification.
        /// </summary>
        public string Message
        {
            get => message;

            set
            {
                if (message == value)
                    return;

                message = value;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationViewModel"/> class with the given values.
        /// </summary>
        /// <param name="profileBrush">The background brush for the profile.</param>
        /// <param name="nameInitials">The profile initials.</param>
        /// <param name="email">The username of the sender.</param>
        /// <param name="message">The message of the notification.</param>
        public NotificationViewModel(ArgbColor profileBrush, string nameInitials, string username, DateTime sentDate, string message)
        {
            ProfileBrush = profileBrush;
            NameInitials = nameInitials;
            Username = username;
            SentDate = sentDate;
            Message = message;
        }

        #endregion
    }
}
