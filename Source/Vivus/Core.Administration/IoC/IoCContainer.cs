﻿namespace Vivus.Core.Administration.IoC
{
    using Ninject;
    using Vivus.Core.Administration.ViewModels;
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

            // Bind to a single instance of the application viewmodel
            Kernel.Bind<IApplicationViewModel<Administrator>>().ToConstant(new ApplicationViewModel());
        }

        /// <summary>
        /// Sets up the IoC container, binds all the information required and is ready for use.
        /// </summary>
        public static void Setup()
        {
            // Bind the unit of work
            Kernel.Bind<IUnitOfWork>().ToConstant(new UnitOfWork(new VivusEntities()));

            // Bind the security instance
            Kernel.Bind<ISecurity>().ToConstant(new Security());

            // Bind all required viewmodels
            BindViewModels();
        }

        /// <summary>
        /// Gets an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns></returns>
        public static T Get<T>() => Kernel.Get<T>();

        /// <summary>
        /// Tries to get an instance of the specified service.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns></returns>
        public static T TryGet<T>() => Kernel.TryGet<T>();
    }
}
