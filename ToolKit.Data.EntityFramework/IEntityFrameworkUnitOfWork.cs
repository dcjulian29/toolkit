using System;
using System.Data.Entity.Infrastructure;

namespace ToolKit.Data.EntityFramework
{
    /// <summary>
    /// A interface representing a unit of work within the EntityFramework ORM.
    /// </summary>
    public interface IEntityFrameworkUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets a <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry"/> object for the
        /// given entity providing access to information about the entity and the ability
        /// to perform actions on the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity.</returns>
        DbEntityEntry Entry(object entity);
    }
}
