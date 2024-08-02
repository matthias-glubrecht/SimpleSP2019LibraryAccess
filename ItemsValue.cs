using System.Text.Json.Serialization;

public class ItemsValue
{
    [JsonPropertyName("value")]
    public List<ListItem>? Items { get; set; }
}