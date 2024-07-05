using System.Text.Json.Serialization;

public class Value
{
    [JsonPropertyName("value")]
    public List<Item>? Items { get; set; }
}