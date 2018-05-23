namespace Vivus.Core.DataModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// Represents a generic dispatcher. 
    /// </summary>
    public class DispatcherWrapper : IDispatcherWrapper
    {
        #region Private Members

        private Dispatcher dispatcher;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherWrapper"/> class with the given value.
        /// </summary>
        /// <param name="dispatcher"></param>
        public DispatcherWrapper(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calls an action asynchronously.
        /// </summary>
        /// <param name="callback">The function to call.</param>
        /// <returns></returns>
        public Task InvokeAsync(Action callback)
        {
            return dispatcher.InvokeAsync(callback).Task;
        }

        #endregion
    }
}
