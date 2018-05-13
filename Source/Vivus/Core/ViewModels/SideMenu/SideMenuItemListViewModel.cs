namespace Vivus.Core.ViewModels.SideMenu
{
    using System.Collections.Generic;
    using Vivus.Core.ViewModels;

    /// <summary>
    /// Represents a view model for the overview side menu list.
    /// </summary>
    public class SideMenuItemListViewModel : BaseViewModel
    {
        private string title;

        /// <summary>
        /// Gets or sets the title of the <see cref="SideMenuItemListControl"/>.
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
        /// Gets or sets the side menu list items for the list.
        /// </summary>
        public List<SideMenuItemViewModel> Items { get; set; }
    }
}
