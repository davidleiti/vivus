namespace Vivus.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an interface for a generic repository.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entities inside the repository.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets an entity based on an identificator.
        /// </summary>
        /// <param name="id">The identificator of the entity to get.</param>
        /// <returns>The entity that has the given identificator.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the entity was not found.</exception>
        TEntity this[int id] { get; }

        /// <summary>
        /// Asynchronously returns the only entity of a sequence that satisfies a specific condition.
        /// </summary>
        /// <param name="predicate">The condition the entity has to satisfy.</param>
        /// <returns>The entity that satisfies the condition.</returns>
        /// <exception cref="ArgumentNullException">If the condition is null.</exception>
        /// <exception cref="InvalidOperationException">If the entity was not found.</exception>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets all the entities inside the <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <returns>All the entities inside the <see cref="IRepository{TEntity}"/>.</returns>
        IEnumerable<TEntity> Entities { get; }

        /// <summary>
        /// Gets asynchronously all the entities inside the <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <returns>All the entities inside the <see cref="IRepository{TEntity}"/>.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets the number of entities contained in the <see cref="IRepository{TEntity}"/>.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Searches for a collection of entities based on a filter.
        /// </summary>
        /// <param name="predicate">The filter the entities should pass in order to be selected.</param>
        /// <returns>All the entities that satisfy the filter.</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds an entity to the <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The entity to add to the <see cref="IRepository{TEntity}"/>.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Adds the entities of the specified collection to the <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The collection whose entities should be added to the <see cref="IRepository{TEntity}"/>.</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the first occurrence of a specific entity from the <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The entity to remove from the <see cref="IRepository{TEntity}"/>.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes the first occurrence of the entities of the specified collection from the <see cref="IRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The collection whose entities should be removed from the <see cref="IRepository{TEntity}"/>.</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
