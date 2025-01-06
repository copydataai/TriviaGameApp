using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaGameApp.Models;
using TriviaGameApp.Services;
using Windows.Services.Maps.OfflineMaps;

namespace TriviaGameApp.Views
{
    public partial class LeaderboardPage : ContentPage
    {
        private List<LeaderboardEntry> _allEntries = new();
        private List<LeaderboardEntry> _filteredEntries = new();

        // We track if the user has changed a filter. Once they do, we show the leaderboard.
        private bool _userHasFiltered = false;

        private readonly string[] _randomLoadingMessages =
        {
            "Preparing the epic showdown...",
            "Gathering legendary trivia results...",
            "You're ready to see the greatest trivia players...",
            "Sorting brainy players from the rest..."
        };

        public LeaderboardPage()
        {
            InitializeComponent();

            // Setup filter pickers
            CategoryFilterPicker.ItemsSource = new List<string>
            {
                "All Categories",
                "General Knowledge",
                "Entertainment: Books",
                "Entertainment: Film",
                "Entertainment: Television",
                "Entertainment: Music",
                // ... add more or load dynamically
            };
            CategoryFilterPicker.SelectedIndex = 0;
            CategoryFilterPicker.SelectedIndexChanged += OnFilterChanged;

            DifficultyFilterPicker.ItemsSource = new List<string>
            {
                "All Difficulties", "Easy", "Medium", "Hard"
            };
            DifficultyFilterPicker.SelectedIndex = 0;
            DifficultyFilterPicker.SelectedIndexChanged += OnFilterChanged;

            // Name search
            NameSearchEntry.TextChanged += OnFilterChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Show loading in center
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            RandomLoadingLabel.IsVisible = true;
            RandomLoadingLabel.Text = GetRandomLoadingMessage();

            // Simulate loading for 3 seconds
            await Task.Delay(3000);

            // Hide loader
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            RandomLoadingLabel.IsVisible = false;

            // Load data from file
            _allEntries = LeaderboardFileService.LoadLeaderboard();
            // Do NOT show the table yet (LeaderboardContainer.IsVisible = false).
            // We'll only show after the user changes a filter.
            LeaderboardContainer.IsVisible = true;

            for(int i = 0; i < _allEntries.Count; i++)
            {
                _allEntries[i].Rank = i + 1;
            }
            LeaderboardCollectionView.ItemsSource = _allEntries;
        }

        private string GetRandomLoadingMessage()
        {
            Random r = new();
            int index = r.Next(_randomLoadingMessages.Length);
            return _randomLoadingMessages[index];
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            // The first time the user changes a filter, show the container
            if (!_userHasFiltered)
            {
                _userHasFiltered = true;
                LeaderboardContainer.IsVisible = true; // now the table + filters appear
            }

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // If we have no data or user hasn't loaded anything yet, just skip
            if (_allEntries.Count == 0) return;

            var query = _allEntries.AsEnumerable();

            // Filter by category
            if (CategoryFilterPicker.SelectedIndex > 0)
            {
                string categoryFilter = CategoryFilterPicker.SelectedItem.ToString() ?? "";
                query = query.Where(x => x.Category == categoryFilter);
            }

            // Filter by difficulty
            if (DifficultyFilterPicker.SelectedIndex > 0)
            {
                string diffFilter = DifficultyFilterPicker.SelectedItem.ToString() ?? "";
                query = query.Where(x => 
                    x.Difficulty.Equals(diffFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by name
            string nameSearch = NameSearchEntry.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => 
                    x.Name.Contains(nameSearch, StringComparison.OrdinalIgnoreCase));
            }

            // Sort by points descending
            _filteredEntries = query.OrderByDescending(x => x.TotalPoints).ToList();

            for (int i = 0; i < _filteredEntries.Count; i++)
            {
                _filteredEntries[i].Rank = i + 1;
            }           

            // Bind to CollectionView
            LeaderboardCollectionView.ItemsSource = _filteredEntries;
        }
    }
}
