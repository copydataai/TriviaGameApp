using Microsoft.Maui.Controls;

namespace TriviaGameApp.Views;
public partial class StartPage : ContentPage
{
    public StartPage()
    {
        InitializeComponent();
    }

    private async void OnNewGameClicked(object sender, EventArgs e)
    {
        // Navigate to the PlayersView to set up the game
        await Shell.Current.GoToAsync("PlayersView");
    }

    private async void OnLeaderboardClicked(object sender, EventArgs e)
    {
        // Navigate to LeaderboardPage
        await Shell.Current.GoToAsync("LeaderboardPage");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        // Navigate to SettingsPage
        await Shell.Current.GoToAsync("SettingsPage");
    }
}