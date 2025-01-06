namespace TriviaGameApp.Models;
public class LeaderboardEntry
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Questions { get; set; } = 0;
    
    public int TimeInSeconds { get; set; } = 0; 
    
    public int TotalPoints { get; set; } = 0;
    public string DateOfGame { get; set; } = string.Empty;
}