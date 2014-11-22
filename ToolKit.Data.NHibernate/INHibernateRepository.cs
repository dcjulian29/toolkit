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
    public interface INHibernateRepository<T, TId> : INHibernateReadOnlyRepository<T, TId>, IRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
    }
}
