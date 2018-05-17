namespace Vivus.Core.ViewModels.SideMenu
{
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Represents a view model for each side menu list item in the overview side menu list.
    /// </summary>
    public class SideMenuItemViewModel : BaseViewModel
    {
        #region Private Members

        private string title;
        private Visibility visibility;
        private bool newContent;
        private bool isSelected;
        private static SideMenuItemViewModel selectedItem = new SideMenuItemViewModel(null);

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets the title for the <see cref="SideMenuItemControl"/>.
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
        /// Gets or sets the visibility of the <see cref="SideMenuItemControl"/>.
        /// </summary>
        public Visibility Visibility
        {
            get => visibility;

            set
            {
                if (visibility == value)
                    return;

                visibility = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the number of changes since last visit.
        /// </summary>
        public bool NewContent
        {
            get => newContent;

            set
            {
                if (newContent == value)
                    return;

                newContent = value;

                OnPropertyChanged();

                OnPropertyChanged(nameof(ChangesVisibility));
            }
        }

        /// <summary>
        /// Gets or sets the selection status of the current <see cref="SideMenuItemControl"/>.
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;

            set
            {
                if (isSelected == value)
                    return;

                isSelected = value;

                OnPropertyChanged();

                NewContent = false;

                lock (selectedItem)
                {
                    if (selectedItem != null)
                        selectedItem.IsSelected = false;
                    selectedItem = this;
                }
            }
        }

        /// <summary>
        /// Gets or sets the visibility status of the news cirlce.
        /// </summary>
        public Visibility ChangesVisibility
        {
            get => NewContent ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets or sets the on click command.
        /// </summary>
        public ICommand OpenPageCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SideMenuItemViewModel"/> class with the givn value.
        /// </summary>
        /// <param name="openPageCommand">The action to be executed when the user clicks on the <see cref="SideMenuItemControl"/>.</param>
        public SideMenuItemViewModel(Action openPageCommand)
        {
            OpenPageCommand = new RelayCommand(() => { IsSelected = true; openPageCommand(); });

            new Thread(async () => await ThreadAction()).Start();
        }

        #endregion

        #region Private Methods

        private async Task ThreadAction()
        {
            var randomSeconds = new Random(DateTime.Now.Second);

            while (true)
            {
                int ticks = randomSeconds.Next(0, 50000);
                await Task.Delay(ticks);

                if (!IsSelected)
                {
                    lock (selectedItem)
                    {
                        NewContent = true;
                    }
                }
            }
        }

        #endregion
    }
}
