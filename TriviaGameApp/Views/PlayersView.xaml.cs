using Microsoft.Maui.Controls;
using TriviaGameApp.Models;

namespace TriviaGameApp.Views;
public static class GameState
{
    public static List<Player>? Players { get; set; }
    public static int TotalQuestionsPerPlayer { get; set; } = 10;
    // e.g. time limit, difficulty, etc.
    public static int TimeLimitInSeconds { get; set; } = 10;
}

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

        GameState.Players = players;
        GameState.TotalQuestionsPerPlayer = 10;
        await Shell.Current.GoToAsync($"//GamePage");
    }
}
