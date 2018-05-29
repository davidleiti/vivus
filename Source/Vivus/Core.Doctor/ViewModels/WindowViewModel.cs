namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.Generic;
    using Vivus.Core.Doctor.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.ViewModels.SideMenu;
    using Windows.Theme.Data;

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
        /// Gets or sets the window owner of this viewmodel.
        /// </summary>
        public IWindow Owner { get; set; }

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
        /// <param name="viewModel">The current page viewmodel.</param>
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
                    Title = "Doctor",
                    Items = new List<SideMenuItemViewModel>
                    {
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.Patients)
                        {
                            Title = "Patients"
                        },
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.Requests)
                        {
                            Title = "Requests"
                        },
                        new SideMenuItemViewModel(() => IoCContainer.Get<WindowViewModel>().CurrentPage = ApplicationPage.ManageBlood)
                        {
                            Title = "Manage blood"
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
