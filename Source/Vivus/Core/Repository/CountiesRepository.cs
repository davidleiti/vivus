namespace Vivus.Core.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a custom repository for the counties.
    /// </summary>
    public class CountiesRepository : Repository<County>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CountiesRepository"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public CountiesRepository(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a county instance based on its name.
        /// </summary>
        /// <param name="name">The name of the gender.</param>
        /// <exception cref="System.ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">If no instance was found of the given type.</exception>
        /// <returns></returns>
        public County County(string name)
        {
            return Find(c => c.Name == name).Single();
        }

        #endregion
    }
}
