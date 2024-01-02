namespace Application.DTO.Request;

using System.Text.Json.Serialization;

public class Powerplant
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("efficiency")]
    public float Efficiency { get; set; }

    [JsonPropertyName("pmin")]
    public int Pmin { get; set; }

    [JsonPropertyName("pmax")]
    public int Pmax { get; set; }

    /// <summary>
    /// Real cost MWh based on Efficiency
    /// </summary>
    [JsonIgnore]
    public double Cost { get; set; }

    /// <summary>
    /// Total cost after power produced
    /// </summary>
    [JsonIgnore]
    public double TotalCost { get; set; }

    /// <summary>
    /// Power produced
    /// </summary>
    [JsonIgnore]
    public double PowerProduced { get; set; }
}
