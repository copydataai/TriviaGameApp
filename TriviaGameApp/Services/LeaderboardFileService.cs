using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Storage;
using TriviaGameApp.Models;

namespace TriviaGameApp.Services
{
    public static class LeaderboardFileService
    {
        private const string LeaderboardFileName = "leaderboard.json";

        /// <summary>
        /// Returns the full path to leaderboard.json in the app data directory.
        /// </summary>
        private static string LeaderboardFilePath 
            => Path.Combine(FileSystem.AppDataDirectory, LeaderboardFileName);

        /// <summary>
        /// Loads the entire leaderboard from the JSON file, or returns an empty list if it doesn't exist.
        /// </summary>
        public static List<LeaderboardEntry> LoadLeaderboard()
        {
            try
            {
                if (!File.Exists(LeaderboardFilePath))
                {
                    return new List<LeaderboardEntry>();
                }

                string json = File.ReadAllText(LeaderboardFilePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var list = JsonSerializer.Deserialize<List<LeaderboardEntry>>(json, options);
                return list ?? new List<LeaderboardEntry>();
            }
            catch (Exception ex)
            {
                // Log error, fallback to empty list
                Console.WriteLine($"Error reading leaderboard file: {ex.Message}");
                return new List<LeaderboardEntry>();
            }
        }

        /// <summary>
        /// Saves the given list of leaderboard entries to the JSON file.
        /// </summary>
        public static void SaveLeaderboard(List<LeaderboardEntry> entries)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(entries, options);
                File.WriteAllText(LeaderboardFilePath, json);
            }
            catch (Exception ex)
            {
                // Log error, handle appropriately
                Console.WriteLine($"Error saving leaderboard file: {ex.Message}");
            }
        }
    }
}
