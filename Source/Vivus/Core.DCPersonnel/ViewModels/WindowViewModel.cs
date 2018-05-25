namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System.Collections.Generic;
    using Vivus.Core.DCPersonnel.DataModels;
    using Vivus.Core.DCPersonnel.IoC;
    using Vivus.Core.ViewModels.SideMenu;

    /// <summary>
    /// Represents a viewmodel for the window.
    /// </summary>
    public class WindowViewModel : Core.ViewModels.WindowViewModel
    {
        #region Private Members

        private ApplicationPage currentPage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current page displayed on the window.
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get => currentPage;

            set
            {
                if (currentPage == value)
                    return;

                currentPage = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a flag that indicates whether the login page should animate on load.
        /// </summary>
        public bool OnLoadAnimateLoginPage { get; set; }

        public bool OnUnloadAnimateLoginPage { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowViewModel"/> class with the default values.
        /// </summary>
        public WindowViewModel() : base()
        {
            OnLoadAnimateLoginPage = false;
            OnUnloadAnimateLoginPage = true;
            CurrentPage = ApplicationPage.Login;
            MenuCategories[0].Items[0].IsSelected = true;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Loads the menu categories.
        /// </summary>
        protected override void LoadMenuOptions()
        {
            MenuCategories = new List<SideMenuItemListViewModel>
            {
                new SideMenuItemListViewModel
                {
                    Title = "DC Personnel",
                    Items = new List<SideMenuItemViewModel>
                    {
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.ManageBlood)
                        {
                            Title = "Manage blood"
                        },
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.BloodRequests)
                        {
                            Title = "Blood requests"
                        },
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.BloodDonationRequests)
                        {
                            Title = "Donation requests"
                        },
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.DonationResults)
                        {
                            Title = "Donations results"
                        }
                    }
                },
                new SideMenuItemListViewModel
                {
                    Title = "Account Settings",
                    Items = new List<SideMenuItemViewModel>
                    {
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.Profile)
                        {
                            Title = "Profile"
                        },
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.Notifications)
                        {
                            Title = "Notifications"
                        }
                    }
                }
            };
        }

        #endregion
    }
}
