namespace Vivus.Core.Repository
{
    using System.Data.Entity;
    using System.Linq;
    using Vivus.Core.Model;

    /// <summary>
    /// Represents a custom repository for the blood types.
    /// </summary>
    public class BloodTypesRepository : Repository<BloodType>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BloodTypesRepository"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public BloodTypesRepository(DbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a blood type instance based on its type attribute.
        /// </summary>
        /// <param name="type">The type attribute of the blood type.</param>
        /// <exception cref="System.ArgumentNullException">If the parameter is null.</exception>
        /// <exception cref="System.InvalidOperationException">If no instance was found of the given type.</exception>
        /// <returns></returns>
        public BloodType BloodType(string type)
        {
            return Find(t => t.Type == type).Single();
        }

        #endregion
    }
}
