using Vivus.Core.ViewModels.Base;

namespace Vivus.Core.UnitTests.Dependencies
{
    public class ApplicationViewModel<TEntity> : IApplicationViewModel<TEntity>
        where TEntity : new()
    {
        public TEntity User { get; set; }
    }
}
