namespace Application.DTO.Request;

using System.Text.Json.Serialization;

public class Fuels
{
    /// <summary>
    /// Euro/MWh
    /// </summary>
    [JsonPropertyName("gas(euro/MWh)")]
    public double GasPrice { get; set; }

    /// <summary>
    /// Euro/MWh
    /// </summary>
    [JsonPropertyName("kerosine(euro/MWh)")]
    public double KerosinePrice { get; set; }

    /// <summary>
    /// Euro/Ton
    /// </summary>
    [JsonPropertyName("co2(euro/ton)")]
    public double CO2Price { get; set; }

    /// <summary>
    /// Percent %
    /// </summary>
    [JsonPropertyName("wind(%)")]
    public double WindPercentage { get; set; }
}
