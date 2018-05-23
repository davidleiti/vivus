namespace Vivus.Core.UnitTests.Dependencies
{
    using System;
    using System.Threading.Tasks;
    using Vivus.Core.DataModels;

    public class DispatcherWrapper : IDispatcherWrapper
    {
        public Task InvokeAsync(Action callback)
        {
            callback();
            return Task.Delay(1);
        }
    }
}
