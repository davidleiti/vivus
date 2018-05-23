namespace Vivus.Core.DataModels
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an interface for a generic dispatcher. 
    /// </summary>
    public interface IDispatcherWrapper
    {
        /// <summary>
        /// Calls an action asynchronously.
        /// </summary>
        /// <param name="callback">The action to call.</param>
        /// <returns></returns>
        Task InvokeAsync(Action callback);
    }
}
