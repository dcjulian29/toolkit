using System;

namespace ToolKit.Data.EntityFramework
{
    /// <summary>
    /// Interface for a Entity Framework Repository supporting the specified entity type
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public interface IEntityFrameworkRepository<T, TId> : IEntityFrameworkReadOnlyRepository<T, TId>, IRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
    }
}
