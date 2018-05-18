namespace Vivus.Core.ViewModels
{
    using System.Collections.Generic;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels.SideMenu;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a viewmodel for the window.
    /// </summary>
    public abstract class WindowViewModel : Windows.Theme.Data.WindowViewModel
    {
        #region Private Members

        private Visibility sideMenuVisibility;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the visibility of the window menu.
        /// </summary>
        public Visibility SideMenuVisibility
        {
            get => sideMenuVisibility;

            set
            {
                if (sideMenuVisibility == value)
                    return;

                sideMenuVisibility = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the list of menu categories.
        /// </summary>
        public List<SideMenuItemListViewModel> MenuCategories { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowViewModel"/> class with the default values.
        /// </summary>
        public WindowViewModel() : base()
        {
            LoadMenuOptions();
            MenuCommand = null;
            SideMenuVisibility = Visibility.Hidden;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the menu icon on the window.
        /// </summary>
        public void ShowMenu()
        {
            MenuCommand = new RelayCommand(ClickMenu);
        }

        /// <summary>
        /// Hides the menu icon on the window.
        /// </summary>
        public void HideMenu()
        {
            MenuCommand = null;
            SideMenuVisibility = Visibility.Hidden;
        }

        #endregion

        #region Protected/Private Methods

        /// <summary>
        /// Loads the menu categories.
        /// </summary>
        protected abstract void LoadMenuOptions();

        /// <summary>
        /// Executes the menu command on the click event.
        /// </summary>
        private void ClickMenu()
        {
            SideMenuVisibility ^= Visibility.Hidden;
            VivusConsole.WriteLine("Menu button pressed.");
        }

        #endregion
    }
}
