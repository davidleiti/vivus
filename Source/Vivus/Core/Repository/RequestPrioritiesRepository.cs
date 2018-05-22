namespace Vivus.Core.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a custom repository for the request priorities.
    /// </summary>
    public class RequestPrioritiesRepository : Repository<RequestPriority>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPrioritiesRepository"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public RequestPrioritiesRepository(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a request priority instance based on its type attribute.
        /// </summary>
        /// <param name="type">The type attribute of the request priority.</param>
        /// <exception cref="System.ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">If no instance was found of the given type.</exception>
        /// <returns></returns>
        public RequestPriority RequestPriority(string type)
        {
            return Find(p => p.Type == type).Single();
        }

        #endregion
    }
}
