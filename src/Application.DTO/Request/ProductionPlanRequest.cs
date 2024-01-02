namespace Application.DTO.Request;

using System.Text.Json.Serialization;

public class ProductionPlanRequest
{
    [JsonPropertyName("load")]
    public double Load { get; set; }

    [JsonPropertyName("fuels")]
    public Fuels Fuels { get; set; }

    [JsonPropertyName("powerplants")]
    public List<Powerplant> PowerPlants { get; set; }
}