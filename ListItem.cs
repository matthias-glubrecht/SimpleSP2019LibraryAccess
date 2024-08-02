using System.Text.Json.Serialization;

public struct ListItem
{
    public string FileLeafRef { get; set; }
    public string FileRef { get; set; }
    public string FileSizeDisplay { get; set; }
    public int ID { get; set; }
    public string ProjektId { get; set; }
    public string Title { get; set; }
}
