namespace RTransX.Models;

using System.Text.Json.Serialization;

public class Response {
    [JsonPropertyName("alternatives")]
    public required string[] Alternatives { get; init; }

    [JsonPropertyName("code")]
    public required int Code { get; init; }

    [JsonPropertyName("data")]
    public required string Data { get; init; }

    [JsonPropertyName("id")]
    public required long Id { get; init; }

    [JsonPropertyName("method")]
    public required string Method { get; init; }

    [JsonPropertyName("source_lang")]
    public required string SourceLang { get; init; }

    [JsonPropertyName("target_lang")]
    public required string TargetLang { get; init; }
}
