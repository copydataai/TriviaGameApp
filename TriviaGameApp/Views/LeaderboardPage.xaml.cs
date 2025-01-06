using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using TriviaGameApp.Models;
using TriviaGameApp.Services;

namespace TriviaGameApp.Views
{
    public partial class LeaderboardPage : ContentPage
    {
        private List<LeaderboardEntry> _entries = new();

        public LeaderboardPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Load from file each time we appear (in case a new entry was added elsewhere)
            _entries = LeaderboardFileService.LoadLeaderboard();
            LeaderboardCollectionView.ItemsSource = _entries;
        }

    }
}
