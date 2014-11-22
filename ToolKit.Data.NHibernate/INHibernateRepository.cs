using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// Interface for a NHibernate Repository supporting the specified entity type
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public interface INHibernateRepository<T, TId> : IRepository<T, TId>
        where T : IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Finds all entities that match the property values.
        /// </summary>
        /// <param name="propertyValuePairs">The property value to match.</param>
        /// <returns>all entities that match the property values.</returns>
        IEnumerable<T> FindBy(IDictionary<string, object> propertyValuePairs);

        /// <summary>
        /// Finds the first entity that match the property values if possible.
        /// </summary>
        /// <param name="propertyValuePairs">The property value pairs.</param>
        /// <returns>
        /// If the entity is found, The first entity that match the property values is returned;
        /// otherwise, a default entity is returned.
        /// </returns>
        T FindFirst(IDictionary<string, object> propertyValuePairs);
    }
}
