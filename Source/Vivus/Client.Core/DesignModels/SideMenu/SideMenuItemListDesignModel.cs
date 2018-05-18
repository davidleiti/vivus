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
                new SideMenuItemViewModel(null)
                {
                    Title = "Your Daily Mix",
                    NewContent = true
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Recently Played",
                    NewContent = true
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Songs",
                    IsSelected = true
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Albums",
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Artists",
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Stations"
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Local Files"
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Videos",
                    NewContent = true
                },
                new SideMenuItemViewModel(null)
                {
                    Title = "Podcasts"
                }
            };
        }
    }
}
