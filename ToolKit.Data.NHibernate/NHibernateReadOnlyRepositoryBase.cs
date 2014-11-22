using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// An abstract class that implements the common functions of a read-only repository for use with NHibernate.
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public abstract class NHibernateReadOnlyRepositoryBase<T, TId> 
        : ReadOnlyRepositoryBase<T, TId>, INHibernateReadOnlyRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        /// <param name="unitOfWorkContext">The unit of work context.</param>
        protected NHibernateReadOnlyRepositoryBase(INHibernateUnitOfWork unitOfWorkContext)
            : base(unitOfWorkContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        protected NHibernateReadOnlyRepositoryBase()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="NHibernateReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        ~NHibernateReadOnlyRepositoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the Unit of Work Context.
        /// </summary>
        protected new INHibernateUnitOfWork Context
        {
            get
            {
                return (INHibernateUnitOfWork)base.Context;
            }

            set
            {
                base.Context = value;
            }
        }

        /// <summary>
        /// Finds all entities that match the property values.
        /// </summary>
        /// <param name="propertyValuePairs">The property value to match.</param>
        /// <returns>all entities that match the property values.</returns>
        public IEnumerable<T> FindBy(IDictionary<string, object> propertyValuePairs)
        {
            var criteria = this.Context.CreateCriteria<T>();

            foreach (var pair in propertyValuePairs)
            {
                criteria.Add(Restrictions.Eq(pair.Key, pair.Value));
            }

            return criteria.List<T>();
        }

        /// <summary>
        /// Finds the first entity that match the property values if possible.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        /// <returns>
        /// If the entity is found, The first entity that match the property values is returned;
        /// otherwise, a default entity is returned.
        /// </returns>
        public T FindFirst(IDictionary<string, object> propertyValuePairs)
        {
            var matches = this.FindBy(propertyValuePairs);

            return matches != null && matches.Any() ? matches.ElementAt(0) : default(T);
        }
    }
}
