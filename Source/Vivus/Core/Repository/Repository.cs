namespace Vivus.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents a container that implements the <see cref="IRepository{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entities inside the repository.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public Repository(DbContext dbContext)
        {
            dbSet = dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Gets an entity based on an identificator.
        /// </summary>
        /// <param name="id">The identificator of the entity to get.</param>
        /// <returns>The entity that has the given identificator.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the entity was not found.</exception>
        public TEntity this[int id]
        {
            get
            {
                TEntity entity = dbSet.Find(id);

                if (entity == null)
                    throw new ArgumentOutOfRangeException("No entity was found with the given identificator.");

                return entity;
            }
        }

        /// <summary>
        /// Gets all the entities inside the <see cref="Repository{TEntity}"/>.
        /// </summary>
        /// <returns>All the entities inside the <see cref="Repository{TEntity}"/>.</returns>
        public IEnumerable<TEntity> Entities => dbSet.ToList();

        /// <summary>
        /// Gets the number of entities contained in the <see cref="Repository{TEntity}"/>.
        /// </summary>
        public int Count => dbSet.Count();

        /// <summary>
        /// Gets the number of entities contained in the <see cref="Repository{TEntity}"/>.
        /// </summary>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        /// <summary>
        /// Adds an entity to the <see cref="Repository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The entity to add to the <see cref="Repository{TEntity}"/>.</param>
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Adds the entities of the specified collection to the <see cref="Repository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The collection whose entities should be added to the <see cref="Repository{TEntity}"/>.</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <summary>
        /// Removes the first occurrence of a specific entity from the <see cref="Repository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The entity to remove from the <see cref="Repository{TEntity}"/>.</param>
        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Removes the first occurrence of the entities of the specified collection from the <see cref="Repository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The collection whose entities should be removed from the <see cref="Repository{TEntity}"/>.</param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
