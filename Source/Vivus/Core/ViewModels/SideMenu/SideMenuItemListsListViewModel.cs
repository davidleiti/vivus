namespace Vivus.Core.ViewModels.SideMenu
{
    using Vivus.Core.ViewModels;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a view model for the overview side menu list.
    /// </summary>
    public class SideMenuItemListsListViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the side menu lists for the main list.
        /// </summary>
        public List<SideMenuItemListViewModel> MenuCategories { get; set; }
    }
}
