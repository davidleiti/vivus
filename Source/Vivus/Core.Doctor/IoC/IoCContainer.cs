namespace Vivus.Core.Doctor.IoC
{
    using Ninject;
    using Vivus.Core.Doctor.ViewModels;

    /// <summary>
    /// Represents the IoC container of the application.
    /// </summary>
    public static class IoCContainer
    {
        /// <summary>
        /// Gets the kernel of the IoC container.
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// Binds all singleton viewmodels.
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of the window viewmodel
            Kernel.Bind<WindowViewModel>().ToConstant(new WindowViewModel());
        }

        /// <summary>
        /// Sets up the IoC container, binds all the information required and is ready for use.
        /// </summary>
        public static void Setup()
        {
            // Bind all required viewmodels
            BindViewModels();
        }

        public static T Get<T>() => Kernel.Get<T>();
    }
}
