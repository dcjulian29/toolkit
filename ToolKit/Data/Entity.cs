using System;
using System.Runtime.Serialization;

namespace ToolKit.Data
{
    /// <summary>
    ///  Provides a base class for entities which will be persisted to the database.
    ///  Include an Id property along with a consistent manner for comparing entities.
    /// </summary>
    /// <remarks>
    /// Since nearly all entities will have a Id type of integer, this 
    /// class leverages this assumption.  If you want an entity with a Id type
    /// other than an integer such as string, then
    /// use <see cref = "EntityWithTypedId{TId}" /> instead.
    /// </remarks>
    [Serializable]
    [DataContract]
    public abstract class Entity : EntityWithTypedId<int>, IEntity
    {
    }
}
