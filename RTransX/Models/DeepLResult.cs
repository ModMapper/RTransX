namespace RTransX.Models;

using System.Text.Json.Serialization;

public class DeepLResult {

    [JsonPropertyName("text")]
    public required string Text { get; init; }

    [JsonPropertyName("alternatives")]
    public required string[] Alternatives { get; init; }
}
