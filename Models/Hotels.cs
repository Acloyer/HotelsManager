using System.Text.Json.Serialization;
public class Hotel
{
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }
}
