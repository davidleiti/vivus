namespace Vivus.Core.Administration.ViewModels
{
    using IoC;

    /// <summary>
    /// Rpresents a locator that locates view models from the IoC for use in binding in xaml files.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Gets a singleton instance of the locator.
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// Gets the window view model.
        /// </summary>
        public static WindowViewModel WindowViewModel => IoCContainer.Get<WindowViewModel>();
        
        /// <summary>
        /// Gets the application view model.
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => IoCContainer.Get<ApplicationViewModel>();
    }
}
