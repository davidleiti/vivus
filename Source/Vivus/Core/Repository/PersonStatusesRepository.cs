namespace Vivus.Core.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a custom repository for the person statuses.
    /// </summary>
    public class PersonStatusesRepository : Repository<PersonStatus>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonStatusesRepository"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public PersonStatusesRepository(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a person status instance based on its type attribute.
        /// </summary>
        /// <param name="type">The type attribute of the person status.</param>
        /// <exception cref="System.ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">If no instance was found of the given type.</exception>
        /// <returns></returns>
        public PersonStatus PersonStatus(string type)
        {
            return Find(s => s.Type == type).Single();
        }

        #endregion
    }
}
