using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToolKit.Data.MongoDb
{
    /// <inheritdoc cref="IMongoEntity"/>
    /// <summary>
    /// Provides a base class for entities which will be persisted to a MongoDB Collection. Include
    /// an Id property along with a consistent manner for comparing entities.
    /// </summary>
    [Serializable]
    [DataContract]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MongoEntity : EntityWithTypedId<ObjectId>, IMongoEntity
    {
        /// <inheritdoc/>
        /// <summary>
        /// Gets or sets the internal ID of this entity as persisted by the storage sub-system.
        /// </summary>
        [BsonId]
        [DataMember]
        public new virtual ObjectId Id
        {
            get;
            protected internal set;
        }
    }
}
