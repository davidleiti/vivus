namespace Vivus.Client.Core.DesignModels.SideMenu
{
    using Vivus.Core.ViewModels.SideMenu;

    /// <summary>
    /// Represents the design-time data for a <see cref="SideMenuItemControl"/>.
    /// </summary>
    public class SideMenuItemDesignModel : SideMenuItemViewModel
    {
        /// <summary>
        /// Gets or sets a single instance of the <see cref="SideMenuItemDesignModel"/>.
        /// </summary>
        public static SideMenuItemDesignModel Instance { get; } = new SideMenuItemDesignModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="SideMenuItemDesignModel"/> class with the default values.
        /// </summary>
        public SideMenuItemDesignModel()
        {
            Title = "Profile";
            NewContent = true;
        }
    }
}
