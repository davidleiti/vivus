namespace Vivus.Client.Core.DesignModels.SideMenu
{
    using Vivus.Core.ViewModels.SideMenu;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the design-time data for a <see cref="SideMenuItemListsListControl"/>.
    /// </summary>
    public class SideMenuItemListsListDesignModel : SideMenuItemListsListViewModel
    {
        /// <summary>
        /// Gets or sets a single instance of the <see cref="SideMenuItemListsListDesignModel"/>.
        /// </summary>
        public static SideMenuItemListsListDesignModel Instance { get; } = new SideMenuItemListsListDesignModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="SideMenuItemListDesignModel"/> class with the default values.
        /// </summary>
        public SideMenuItemListsListDesignModel()
        {
            MenuCategories = new List<SideMenuItemListViewModel>
            {
                new SideMenuItemListViewModel
                {
                    Title = "Donor",
                    Items = new List<SideMenuItemViewModel>
                    {
                        new SideMenuItemViewModel(null)
                        {
                            Title = "Dashboard"
                        },
                        new SideMenuItemViewModel(null)
                        {
                            Title = "Apply"
                        },
                        new SideMenuItemViewModel(null)
                        {
                            Title = "History"
                        }
                    }
                },
                new SideMenuItemListViewModel
                {
                    Title = "Account Settings",
                    Items = new List<SideMenuItemViewModel>
                    {
                        new SideMenuItemViewModel(null)
                        {
                            Title = "Profile"
                        },
                        new SideMenuItemViewModel(null)
                        {
                            Title = "Notifications"
                        }
                    }
                }
            };

            MenuCategories[0].Items[0].IsSelected = true;
        }
    }
}
