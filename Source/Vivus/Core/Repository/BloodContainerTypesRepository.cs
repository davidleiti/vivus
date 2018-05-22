namespace Vivus.Core.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a custom repository for the blood container types.
    /// </summary>
    public class BloodContainerTypesRepository : Repository<BloodContainerType>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BloodContainerTypesRepository"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public BloodContainerTypesRepository(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a blood container type instance based on its type attribute.
        /// </summary>
        /// <param name="type">The type attribute of the blood container type.</param>
        /// <exception cref="System.ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">If no instance was found of the given type.</exception>
        /// <returns></returns>
        public BloodContainerType BloodContainerType(string type)
        {
            return Find(c => c.Type == type).Single();
        }

        #endregion
    }
}
