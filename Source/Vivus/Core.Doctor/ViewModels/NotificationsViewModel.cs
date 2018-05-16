namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Notifications;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the notifications page.
    /// </summary>
    public class NotificationsViewModel : BaseViewModel
    {
        private string nin;
        private string message;

        public IPage ParentPage { get; set; }


        public List<BasicEntity<string>> PersonTypes { get; }
        public BasicEntity<string> SelectedPersonType { get; set; }


        public List<BasicEntity<string>> PersonNames { get; }
        public BasicEntity<string> SelectedPersonName { get; set; }

        public string Nin
        {
            get => nin;

            set
            {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

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

        /// <summary>
        /// Gets the send command.
        /// </summary>
        public ICommand SendCommand { get; }


        /// <summary>
        /// Gets the collection of the notifications.
        /// </summary>
        public ObservableCollection<NotificationViewModel> Items { get; }


        public NotificationsViewModel()
        {
            PersonTypes = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select person muie") };
            PersonNames = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select person fk") };

            nin = "123";
            message = "sal";

            SendCommand = new RelayCommand(Send);

            Items = new ObservableCollection<NotificationViewModel>();
            // Test whether the binding was done right or not
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "AP", "andreipopescu", new DateTime(2018, 4, 28), "This is dă message."));
            });
        }

        /// <summary>
        /// Send a message.
        /// </summary>
        private void Send()
        {
         
            Vivus.Console.WriteLine("Doctor:  Message sent!");
            Popup("Successfull operation!", PopupType.Successful);
        }
    }
}
