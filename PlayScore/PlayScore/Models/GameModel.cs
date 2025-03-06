using Newtonsoft.Json;

namespace PlayScore.Models;

public sealed class GameModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("released")]
    public DateOnly Released { get; set; }

    [JsonProperty("rating")]
    public double Rating { get; set; }

    public int MondphaseID { get; set; }
}
