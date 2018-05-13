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
        private string title;
        private bool newContent;
        private bool isSelected;
        private static SideMenuItemViewModel selectedItem = new SideMenuItemViewModel();

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

        public Visibility ChangesVisibility
        {
            get => NewContent ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand OpenPageCommand { get; }

        public SideMenuItemViewModel()
        {
            OpenPageCommand = new RelayCommand(() => { OpenPage(); });
            Thread t = new Thread(async () => await ThreadAction());

            t.Start();
        }

        private void OpenPage()
        {
            //NewContentCount = 0;
            IsSelected = true;
        }

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
    }
}
