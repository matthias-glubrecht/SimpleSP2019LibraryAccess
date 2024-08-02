using System.Text.Json.Serialization;

public class FieldsValue
{
    [JsonPropertyName("value")]
    public List<Field>? Fields { get; set; }
}