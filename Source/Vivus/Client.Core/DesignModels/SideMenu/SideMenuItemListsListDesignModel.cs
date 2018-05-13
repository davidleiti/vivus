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
            Lists = new List<SideMenuItemListViewModel>
            {
                new SideMenuItemListViewModel
                {
                    Title = "Donor2",
                    Items = new List<SideMenuItemViewModel>
                    {
                        new SideMenuItemViewModel
                        {
                            Title = "Dashboard"
                        },
                        new SideMenuItemViewModel
                        {
                            Title = "Apply"
                        },
                        new SideMenuItemViewModel
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
                        new SideMenuItemViewModel
                        {
                            Title = "Profile"
                        },
                        new SideMenuItemViewModel
                        {
                            Title = "Notifications"
                        }
                    }
                }
            };

            Lists[0].Items[0].IsSelected = true;
        }
    }
}
