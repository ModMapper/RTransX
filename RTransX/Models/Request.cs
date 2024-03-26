namespace RTransX.Models;

using System.Text.Json.Serialization;

public class Request {
    [JsonPropertyName("text")]
    public required string Text { get; init; }
    [JsonPropertyName("source_lang")]
    public required string SourceLang { get; init; }
    [JsonPropertyName("target_lang")]
    public required string TargetLang { get; init; }
}
