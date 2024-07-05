using System.Text.Json.Serialization;

public class Item
{
    [JsonPropertyName("FileRef")]
    public string? FileRef { get; set; }

    [JsonPropertyName("FileLeafRef")]
    public string? FileLeafRef { get; set; }
}
