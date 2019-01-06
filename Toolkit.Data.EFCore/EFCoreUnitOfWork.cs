using System.Linq;
using Common.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using ToolKit.Data;

namespace Toolkit.Data.EFCore
{
    /// <inheritdoc cref="DbContext"/>
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing
    /// out of changes and the resolution of concurrency problems.
    /// </summary>
    public class EfCoreUnitOfWork : DbContext, IUnitOfWork
    {
        private static ILog _log = LogManager.GetLogger<EfCoreUnitOfWork>();

        private readonly IDbContextTransaction _transaction;
        private bool _rollbackOnDispose;

        /// <inheritdoc/>
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Toolkit.Data.EFCore.EfCoreUnitOfWork"/> class.
        /// </summary>
        /// <param name="options">The options to be used by this unit of work</param>
        /// <param name="rollbackOnDispose">
        /// Should a roll back occur if this class is disposed before commit.
        /// </param>
        public EfCoreUnitOfWork(DbContextOptions options, bool rollbackOnDispose = false) : base(options)
        {
            _rollbackOnDispose = rollbackOnDispose;
            _transaction = base.Database.BeginTransaction();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public new void Attach<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
            Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Deletes the specified entity from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
            Entry(entity).State = EntityState.Deleted;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// non-managed resources.
        /// </summary>
        public new void Dispose()
        {
            if (_rollbackOnDispose)
            {
                _log.Warn("Rolling back Unit Of Work Transaction...");
                _transaction.Rollback();
            }
            else
            {
                SaveChanges();
                _transaction.Commit();
            }

            _transaction.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Gets a <see cref="T:System.Data.Entity.Infrastructure.EntityEntry"/> object for the given
        /// entity providing access to information about the entity and the ability to perform
        /// actions on the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity.</returns>
        public new EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Gets this instance from the EntityFramework Context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An instance of the entity that can be used by the Repository implementation to further
        /// query the results.
        /// </returns>
        public IQueryable<T> Get<T>() where T : class
        {
            return Set<T>();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Mark this unit of work to be rollback.
        /// </summary>
        public void Rollback()
        {
            _rollbackOnDispose = true;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Saves the specified entity to the EntityFramework Context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Save<T>(T entity) where T : class
        {
            var method = entity.GetType().GetMethod("IsTransient");

            if (method != null)
            {
                var transient = (bool)method.Invoke(entity, null);

                Entry(entity).State = transient ? EntityState.Added : EntityState.Modified;
            }

            if (Entry(entity).State == EntityState.Added)
            {
                Set<T>().Add(entity);
            }
        }
    }
}
