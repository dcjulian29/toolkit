using System;

namespace ToolKit.Data
{
    /// <summary>
    /// Interface that represents an entity within the data model with
    /// an integer as its internal ID property.
    /// </summary>
    public interface IEntity : IEntityWithTypedId<int>
    {
    }
}
