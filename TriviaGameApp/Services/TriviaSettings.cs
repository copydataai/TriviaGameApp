using Microsoft.Maui.Storage;
using System;

namespace TriviaGameApp.Services;
public static class TriviaSettings
{
    // Current in-memory values (defaults)
    public static string Category { get; set; } = "any";   // or "9", etc.
    public static string Difficulty { get; set; } = "any"; // "easy", "medium", "hard", "any"
    public static string TriviaType { get; set; } = "any"; // "multiple", "boolean", "any"
    public static int NumberOfQuestions { get; set; } = 10;

    public static string AppThemeChoice { get; set; } = "Dark";

    // Keys used in Preferences
    private const string CategoryKey = "Category";
    private const string DifficultyKey = "Difficulty";
    private const string TriviaTypeKey = "TriviaType";
    private const string NumQuestionsKey = "NumberOfQuestions";

    private const string AppThemeChoiceKey = "AppThemeChoice";

    public static void LoadSettings()
    {
        bool fileLoaded = false;
        try
        {
            // Try loading from file first
            var data = SettingsFileService.LoadSettingsFromFile();
            if (data != null)
            {
                FromData(data);
                fileLoaded = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
        }

        // If file not loaded, try Preferences
        if (!fileLoaded)
        {
            Console.WriteLine("Falling back to Preferences for settings.");
            Category = Preferences.Get(CategoryKey, "any");
            Difficulty = Preferences.Get(DifficultyKey, "any");
            TriviaType = Preferences.Get(TriviaTypeKey, "any");
            NumberOfQuestions = Preferences.Get(NumQuestionsKey, 10);
            AppThemeChoice = Preferences.Get(AppThemeChoiceKey, "Dark");
        }
    }

    /// Saves current settings to:
    /// 1. The JSON file (trivia_settings.json)
    /// 2. Preferences
    public static void SaveSettings()
    {
        // 1. Save to JSON file
        var data = ToData();
        SettingsFileService.SaveSettingsToFile(data);

        // 2. Save to Preferences
        Preferences.Set(CategoryKey, Category);
        Preferences.Set(DifficultyKey, Difficulty);
        Preferences.Set(TriviaTypeKey, TriviaType);
        Preferences.Set(NumQuestionsKey, NumberOfQuestions);
        Preferences.Set(AppThemeChoiceKey, AppThemeChoice);
    }

    public static void FromData(TriviaSettingsData data)
    {
        Category = data.Category;
        Difficulty = data.Difficulty;
        TriviaType = data.TriviaType;
        NumberOfQuestions = data.NumberOfQuestions;
        AppThemeChoice = data.AppThemeChoice;
    }

    public static TriviaSettingsData ToData() => new()
    {
        Category = Category,
        Difficulty = Difficulty,
        TriviaType = TriviaType,
        NumberOfQuestions = NumberOfQuestions,
        AppThemeChoice = AppThemeChoice
    };
}

/// Data class for JSON serialization/deserialization
public class TriviaSettingsData
{
    public string Category { get; set; } = "any";
    public string Difficulty { get; set; } = "any";
    public string TriviaType { get; set; } = "any";
    public int NumberOfQuestions { get; set; } = 10;
    

    public string AppThemeChoice { get; set; } = "Dark";
}
