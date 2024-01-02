namespace Application.DTO.Response;

using System.Text.Json.Serialization;

public class PowerPlantResult
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Power
    /// </summary>
    [JsonPropertyName("p")]
    public double Power { get; set; }

    /// <summary>
    /// Total cost
    /// </summary>
    [JsonIgnore]
    public double TotalCost { get; set; }
}