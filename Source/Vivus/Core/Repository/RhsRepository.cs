namespace Vivus.Core.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a custom repository for the RHs.
    /// </summary>
    public class RhsRepository : Repository<RH>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RhsRepository"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public RhsRepository(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an RH instance based on its type attribute.
        /// </summary>
        /// <param name="type">The type attribute of the RH.</param>
        /// <exception cref="System.ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">If no instance was found of the given type.</exception>
        /// <returns></returns>
        public RH RH(string type)
        {
            return Find(rh => rh.Type == type).Single();
        }

        #endregion
    }
}
