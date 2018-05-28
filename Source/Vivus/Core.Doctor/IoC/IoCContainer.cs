namespace Vivus.Core.Doctor.IoC
{
    using Ninject;
    using Vivus.Core.Doctor.ViewModels;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels.Base;

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
            // Bind to a single instance of the UnitOfWork
            Kernel.Bind<IUnitOfWork>().ToConstant(new UnitOfWork(new VivusEntities()));
            // Bind to a single instance of the Security
            Kernel.Bind<ISecurity>().ToConstant(new Security());
            // Bind to a single instance of the application viewmodel
            Kernel.Bind<IApplicationViewModel<Doctor>>().ToConstant(new ApplicationViewModel());
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
