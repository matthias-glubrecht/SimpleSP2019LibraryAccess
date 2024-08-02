using System.Text.Json.Serialization;

public struct RequestError
{
    [JsonPropertyName("odata.error")]
    public OdataError OdataError { get; set; }
}

public struct OdataError
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("message")]
    public Message Message { get; set; }

}

public struct Message
{
    [JsonPropertyName("lang")]
    public string Lang { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
}