namespace Vivus.Client.Core.DesignModels.Notifications
{
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels.Notifications;
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a view model for a notifications panel.
    /// </summary>
    public class NotificationsPanelDesignModel : NotificationsPanelViewModel
    {
        public static NotificationsPanelDesignModel Instance { get; } = new NotificationsPanelDesignModel();

        public NotificationsPanelDesignModel()
        {
            Title = "Recent notifications";

            Items = new ObservableCollection<NotificationViewModel>
            {
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 5, 4), "WAKANDA FOREVER!!!"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 3, 21), "Pls tell no police"),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 3, 21), "Jesus!"),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 3, 20), "None?"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 3, 20), "Wanna try?"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 3, 20), "Limited offer"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 3, 19), "I got something fo yah"),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 3, 15), "Hable romanes?"),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 3, 12), "No? Spanish?"),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 3, 11), "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 3, 11), "Latin?"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 2, 8), "Stop pretending"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 2, 8), "I kno u there. I aint no dumb"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 2, 1), "No? OK"),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2018, 1, 27), "Ni**a?!"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 1, 23), "How about a lung?"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 1, 22), "Fine!"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 1, 22), "Pls respond"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 1, 21), "Offer is fading"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 1, 20), "U better be quick"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 1, 19), "U want a kidney?"),
                new NotificationViewModel(new ArgbColor(255, 232, 62, 140), "AP", "andreipopescu", new DateTime(2018, 1, 5), "Nigga u there?"),
                new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "DP", "davidperisanu", new DateTime(2017, 12, 12), "Wanna donate blood?")
            };
        }
    }
}
