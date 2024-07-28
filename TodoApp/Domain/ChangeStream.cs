using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApp.Domain;

public class ChangeStream<T>
{
    [BsonElement("_id")]
    public ChangeStreamDocumentId Id { get; set; }

    [BsonElement("operationType")]
    public string OperationType { get; set; }

    [BsonElement("clusterTime")]
    public DateTime ClusterTime { get; set; }

    [BsonElement("documentKey")]
    public DocumentKey DocumentKey { get; set; }

    [BsonElement("updateDescription")]
    public UpdateDescription UpdateDescription { get; set; }

    [BsonElement("fullDocument")]
    public T FullDocument { get; set; }
}

public class ChangeStreamDocumentId
{
    [BsonElement("_data")]
    public string Data { get; set; }
}

public class DocumentKey
{
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
}

public class UpdateDescription
{
    [BsonElement("updatedFields")]
    public Dictionary<string, object> UpdatedFields { get; set; }

    [BsonElement("removedFields")]
    public List<string> RemovedFields { get; set; }

    [BsonElement("truncatedArrays")]
    public List<string> TruncatedArrays { get; set; }
}
