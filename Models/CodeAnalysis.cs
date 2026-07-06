using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VibeCodeAI.Models;

public class CodeAnalysis
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string Agent { get; set; } = string.Empty;

    public string Response { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    // Optional - Useful if you later support multiple projects
    public string ProjectName { get; set; } = "Default Project";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}