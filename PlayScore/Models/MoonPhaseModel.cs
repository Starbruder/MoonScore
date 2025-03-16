using Newtonsoft.Json;

namespace MoonScore.Models;

public sealed class MoonPhaseModel
{
    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("moon_phase")]
    public string MoonPhase { get; set; }

    [JsonProperty("moon_illumination")]
    public double MoonIllumination { get; set; }

    [JsonProperty("moonrise")]
    public string Moonrise { get; set; }

    [JsonProperty("moonset")]
    public string Moonset { get; set; }

    [JsonProperty("sunrise")]
    public string Sunrise { get; set; }

    [JsonProperty("sunset")]
    public string Sunset { get; set; }
}
