using System.Text.Json.Serialization;

namespace TriviaGameApp.Models;
public class LeaderboardEntry
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Questions { get; set; } = 0;

    public string Difficulty { get; set; } = string.Empty;
    
    public int TimeInSeconds { get; set; } = 0; 
    
    public int TotalPoints { get; set; } = 0;
    public string DateOfGame { get; set; } = string.Empty;

    [JsonIgnore]
    public int Rank { get; set; } = 0;

    [JsonIgnore]
    public string MedalEmoji => Rank switch
    {
        1 => "🥇",
        2 => "🥈",
        3 => "🥉",
        _ => string.Empty
    };
}