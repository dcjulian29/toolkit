using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit.Data.EntityFramework
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and 
    /// coordinates the writing out of changes and the resolution of 
    /// concurrency problems.
    /// </summary>
    public class EntityFrameworkUnitOfWork : IEntityFrameworkUnitOfWork
    {
        private static Common.Logging.ILog _log = Common.Logging.LogManager.GetCurrentClassLogger();

        private bool _rollbackOnDispose = false;

        private DbContext _context;

        private DbContextTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkUnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The EntityFramework Context.</param>
        public EntityFrameworkUnitOfWork(DbContext context)
        {
            _context = context;
            _transaction = _context.Database.BeginTransaction();
        }

        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Attach<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_rollbackOnDispose)
            {
                _log.Warn("Rolling back Unit Of Work Transaction...");
                _transaction.Rollback();
            }
            else
            {
                _transaction.Commit();
            }

            _transaction.Dispose();
            _context.Dispose();
        }

        /// <summary>
        /// Gets this instance from the EntityFramework Session.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An IQueryable instance of the entity that can be used by the Repository implementation.
        /// </returns>
        public IQueryable<T> Get<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        /// <summary>
        /// Mark this unit of work to be rollback.
        /// </summary>
        public void Rollback()
        {
            _rollbackOnDispose = true;
        }

        /// <summary>
        /// Saves the specified entity to the EntityFramework Session.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Save<T>(T entity) where T : class
        {
            _context.SaveChanges();
        }
    }
}
