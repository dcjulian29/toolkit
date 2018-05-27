using MongoDB.Bson;

namespace ToolKit.Data.MongoDb
{
    /// <inheritdoc/>
    /// <summary>
    /// This interface serves as the base interface for entities stored in a MongoDB instance.
    /// </summary>
    public interface IMongoEntity : IEntityWithTypedId<ObjectId>
    {
    }
}
