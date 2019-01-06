using System;
using System.Linq;
using Common.Logging;
using NHibernate;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing
    /// out of changes and the resolution of concurrency problems.
    /// </summary>
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private static ILog _log = LogManager.GetLogger<NHibernateUnitOfWork>();

        private readonly ISession _session;
        private readonly ITransaction _transaction;
        private bool _rollbackOnDispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateUnitOfWork"/> class.
        /// </summary>
        /// <param name="session">The NHibernate Session.</param>
        public NHibernateUnitOfWork(ISession session)
        {
            _session = session;

            _transaction = _session.BeginTransaction();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Attach<T>(T entity) where T : class
        {
            _session.Update(entity);
        }

        /// <summary>
        /// Creates an NHibernate Criteria instance.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>An NHibernate Criteria instance.</returns>
        public ICriteria CreateCriteria<T>() where T : class
        {
            return _session.CreateCriteria<T>();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Deletes the specified entity from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<T>(T entity) where T : class
        {
            _session.Delete(entity);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Gets this instance from the NHibernate Session.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An instance of the entity that can be used by the Repository implementation to further
        /// query the results.
        /// </returns>
        public IQueryable<T> Get<T>() where T : class
        {
            return _session.Query<T>();
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
        /// Saves the specified entity to the NHibernate Session.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Save<T>(T entity) where T : class
        {
            _session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Releases non-managed and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and non-managed resources; <c>false</c> to release
        /// only non-managed resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_transaction.IsActive)
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
            }

            _session.Close();
            _transaction.Dispose();
            _session.Dispose();
        }
    }
}
