namespace Vivus.Core.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a custom repository for the genders.
    /// </summary>
    public class GendersRepository : Repository<Gender>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GendersRepository"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public GendersRepository(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a gender instance based on its type.
        /// </summary>
        /// <param name="type">The type of the gender.</param>
        /// <exception cref="System.ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">If no instance was found of the given type.</exception>
        /// <returns></returns>
        public Gender Gender(string type)
        {
            return Find(g => g.Type == type).Single();
        }

        #endregion
    }
}
