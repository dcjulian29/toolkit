using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Common.Logging;

namespace ToolKit.Data.EntityFramework
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and 
    /// coordinates the writing out of changes and the resolution of 
    /// concurrency problems.
    /// </summary>
    public class EntityFrameworkUnitOfWork : DbContext, IUnitOfWork
    {
        private static ILog _log = LogManager.GetLogger<EntityFrameworkUnitOfWork>();

        private bool _rollbackOnDispose = false;

        private DbContextTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkUnitOfWork"/> class.
        /// </summary>
        public EntityFrameworkUnitOfWork()
        {
            _transaction = Database.BeginTransaction();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkUnitOfWork"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string. </param>
        public EntityFrameworkUnitOfWork(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            _transaction = Database.BeginTransaction();
        }

        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Attach<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
            Entry(entity).State = EntityState.Modified;
        }

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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
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
        /// Gets a <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry"/> object for the
        /// given entity providing access to information about the entity and the ability
        /// to perform actions on the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity.</returns>
        public new DbEntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        /// <summary>
        /// Gets this instance from the EntityFramework Context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An instance of the entity that can be used by the Repository implementation to further query the results.
        /// </returns>
        public IQueryable<T> Get<T>() where T : class
        {
            return Set<T>();
        }

        /// <summary>
        /// Mark this unit of work to be rollback.
        /// </summary>
        public void Rollback()
        {
            _rollbackOnDispose = true;
        }

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
