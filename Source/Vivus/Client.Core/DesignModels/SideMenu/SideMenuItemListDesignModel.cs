namespace Vivus.Client.Core.DesignModels.SideMenu
{
    using Vivus.Core.ViewModels.SideMenu;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the design-time data for a <see cref="SideMenuItemListControl"/>.
    /// </summary>
    public class SideMenuItemListDesignModel : SideMenuItemListViewModel
    {
        /// <summary>
        /// Gets or sets a single instance of the <see cref="SideMenuItemListDesignModel"/>.
        /// </summary>
        public static SideMenuItemListDesignModel Instance { get; } = new SideMenuItemListDesignModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="SideMenuItemListDesignModel"/> class with the default values.
        /// </summary>
        public SideMenuItemListDesignModel()
        {
            Title = "Your Library";
            Items = new List<SideMenuItemViewModel>
            {
                new SideMenuItemViewModel
                {
                    Title = "Your Daily Mix",
                    NewContent = true
                },
                new SideMenuItemViewModel
                {
                    Title = "Recently Played",
                    NewContent = true
                },
                new SideMenuItemViewModel
                {
                    Title = "Songs",
                    IsSelected = true
                },
                new SideMenuItemViewModel
                {
                    Title = "Albums",
                },
                new SideMenuItemViewModel
                {
                    Title = "Artists",
                },
                new SideMenuItemViewModel
                {
                    Title = "Stations"
                },
                new SideMenuItemViewModel
                {
                    Title = "Local Files"
                },
                new SideMenuItemViewModel
                {
                    Title = "Videos",
                    NewContent = true
                },
                new SideMenuItemViewModel
                {
                    Title = "Podcasts"
                }
            };
        }
    }
}
