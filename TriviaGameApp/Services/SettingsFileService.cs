using System;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace TriviaGameApp.Services
{
    public static class SettingsFileService
    {
        private const string SettingsFileName = "trivia_settings.json";

        private static string SettingsFilePath 
            => Path.Combine(FileSystem.AppDataDirectory, SettingsFileName);

        public static TriviaSettingsData? LoadSettingsFromFile()
        {
            if (!File.Exists(SettingsFilePath))
            {
                return null; 
            }

            var json = File.ReadAllText(SettingsFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<TriviaSettingsData>(json, options);
        }

        public static void SaveSettingsToFile(TriviaSettingsData data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(SettingsFilePath, json);
        }
    }
}

