using System;
using System.Linq;
using Common.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace ToolKit.Data.MongoDb
{
    /// <inheritdoc/>
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing
    /// out of changes and the resolution of concurrency problems.
    /// </summary>
    public class MongoDbUnitOfWork : IUnitOfWork
    {
        private static ILog _log = LogManager.GetLogger<MongoDbUnitOfWork>();

        private readonly MongoDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbUnitOfWork"/> class.
        /// </summary>
        /// <param name="database">The Mongo Database instance.</param>
        public MongoDbUnitOfWork(MongoDatabase database) => _database = database;

        /// <inheritdoc/>
        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Attach<T>(T entity) where T : class => Save(entity);

        /// <inheritdoc/>
        /// <summary>
        /// Deletes the specified entity from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<T>(T entity) where T : class
        {
            var result = GetCollection<T>().Remove(Query.EQ("_id", GetMongoEntity(entity).Id));

            if (result.HasLastErrorMessage)
            {
                _log.Error(m => m("Error occurred during Delete... {0}", result.LastErrorMessage));
            }
        }

        /// <inheritdoc/>
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// non-managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        /// <summary>
        /// Gets this instance from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An instance of the entity that can be used by the Repository implementation to further
        /// query the results.
        /// </returns>
        public IQueryable<T> Get<T>() where T : class => GetCollection<T>().FindAllAs<T>().AsQueryable();

        /// <inheritdoc/>
        /// <summary>
        /// Mark this unit of work to be rollback.
        /// </summary>
        public void Rollback() => _log.Warn(m => m("MongoDB does not support transactions..."));

        /// <inheritdoc/>
        /// <summary>
        /// Saves the specified entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Save<T>(T entity) where T : class => GetCollection<T>().Save(entity);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Method intentionally left empty.
        }

        private MongoCollection GetCollection<T>() => _database.GetCollection<T>(typeof(T).Name.ToLower());

        private IMongoEntity GetMongoEntity<T>(T entity) where T : class
        {
            var entityType = entity.GetType();

            if (!typeof(IMongoEntity).IsAssignableFrom(entityType))
            {
                throw new ArgumentException($"{entityType.Name} is not assignable to IMongoEntity.");
            }

            return (IMongoEntity)entity;
        }
    }
}
