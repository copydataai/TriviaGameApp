using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TriviaGameApp.Models;
public class OpenTDBResponse
{
    [JsonPropertyName("response_code")]
    public int ResponseCode { get; set; }

    [JsonPropertyName("results")]
    public List<OpenTDBQuestion>? Results { get; set; }
}

public class OpenTDBQuestion
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("difficulty")]
    public string? Difficulty { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("question")]
    public string? Question { get; set; }

    [JsonPropertyName("correct_answer")]
    public string? CorrectAnswer { get; set; }

    [JsonPropertyName("incorrect_answers")]
    public List<string>? IncorrectAnswers { get; set; }
}

public class TriviaQuestion
{
    public string? Category { get; set; }
    public string? Difficulty { get; set; }
    public string? Question { get; set; }
    public string? CorrectAnswer { get; set; }
    
    public List<string> AllAnswers { get; set; } = new();
}
