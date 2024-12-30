using Microsoft.Maui.Controls;
using TriviaGameApp.Models;

namespace TriviaGameApp.Views;

public partial class PlayersView : ContentPage
{
    // Store the entries so we can read them later
    private List<Entry> _playerNameEntries = new();

    public PlayersView()
    {
        InitializeComponent();
    }

    private void OnPlayerCountChanged(object sender, EventArgs e)
    {
        PlayerNamesContainer.Children.Clear();
        _playerNameEntries.Clear();

        // Get the selected # of players from the Picker
        int selectedCount = (int)PlayerCountPicker.SelectedItem;

        for (int i = 1; i <= selectedCount; i++)
        {
            var entry = new Entry
            {
                Placeholder = $"Player {i} Name"
            };
            _playerNameEntries.Add(entry);
            PlayerNamesContainer.Children.Add(entry);
        }
    }

    private async void OnStartGameClicked(object sender, EventArgs e)
    {
        int selectedCount = (int)PlayerCountPicker.SelectedItem;
        var players = new List<Player>();

        for (int i = 0; i < selectedCount; i++)
        {
            players.Add(new Player
            {
                Name = _playerNameEntries[i].Text ?? $"Player{i+1}",
                Score = 0
            });
        }

        // At this point, you have the players and their names.
        // If there's only 1 player, you can start the single-player flow.
        // If multiple players, you can do round-robin or per-round logic.
        
        // For now, let's just navigate to a "GamePage" (not shown) and pass these players.
        // E.g., use a query property or store in a shared service.
        
        // Example: Passing data via query parameters (simplified example).
        // In a real scenario, you might implement a data service or an actual route with query.
        // For brevity, assume we have a route "GamePage" registered in AppShell.
        // Also note that you can’t pass complex objects via query in Shell easily.
        // You’d typically use a shared static service or an observable singleton with a binding.

        // Pseudocode if you had a service:
        // MyGameService.CurrentPlayers = players;
        // await Shell.Current.GoToAsync("GamePage");

        await DisplayAlert("Players", 
            $"Starting game for {selectedCount} player(s)!", "OK");
    }
}
