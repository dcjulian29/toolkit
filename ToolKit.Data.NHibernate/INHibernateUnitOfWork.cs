using NHibernate;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// A interface representing a unit of work within the NHibernate ORM.
    /// </summary>
    public interface INHibernateUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Creates an NHibernate Criteria instance.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An NHibernate Criteria instance.
        /// </returns>
        ICriteria CreateCriteria<T>() where T : class;
    }
}
