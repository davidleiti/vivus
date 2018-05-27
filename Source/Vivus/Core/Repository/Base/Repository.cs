﻿namespace Vivus.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a container that implements the <see cref="IRepository{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entities inside the repository.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        #region Protected Members

        protected readonly DbSet<TEntity> dbSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class with the given value.
        /// </summary>
        /// <param name="dbContext">The Unit of Work for the Entity Framework instance.</param>
        public Repository(DbContext dbContext)
        {
            dbSet = dbContext.Set<TEntity>();
        }

        #endregion

        #region Public Methods

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
        /// Asynchronously returns the only entity of a sequence that satisfies a specific condition.
        /// </summary>
        /// <param name="predicate">The condition the entity has to satisfy.</param>
        /// <returns>The entity that satisfies the condition.</returns>
        /// <exception cref="ArgumentNullException">If the condition is null.</exception>
        /// <exception cref="InvalidOperationException">If the entity was not found.</exception>
        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.SingleAsync(predicate);
        }

        /// <summary>
        /// Gets all the entities inside the <see cref="Repository{TEntity}"/>.
        /// </summary>
        /// <returns>All the entities inside the <see cref="Repository{TEntity}"/>.</returns>
        public IEnumerable<TEntity> Entities => dbSet.ToList();

        /// <summary>
        /// Gets asynchronously all the entities inside the <see cref="Repository{TEntity}"/>.
        /// </summary>
        /// <returns>All the entities inside the <see cref="Repository{TEntity}"/>.</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        /// <summary>
        /// Gets the number of entities contained in the <see cref="Repository{TEntity}"/>.
        /// </summary>
        public int Count => dbSet.Count();

        /// <summary>
        /// Searches for a collection of entities based on a filter.
        /// </summary>
        /// <param name="predicate">The filter the entities should pass in order to be selected.</param>
        /// <returns>All the entities that satisfy the filter.</returns>
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

        #endregion
    }
}
