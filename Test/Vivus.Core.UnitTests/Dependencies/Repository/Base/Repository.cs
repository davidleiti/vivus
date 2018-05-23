namespace Vivus.Core.UnitTests.Dependencies.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Vivus.Core.Repository;

    /// <summary>
    /// Represents a container that implements the <see cref="IRepository{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entities inside the repository.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        public TEntity this[int id] => new TEntity();

        public IEnumerable<TEntity> Entities => new List<TEntity>();

        public int Count => 0;

        public void Add(TEntity entity)
        {
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return new List<TEntity> { new TEntity() };
        }

        public void Remove(TEntity entity)
        {
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
        }
    }
}
