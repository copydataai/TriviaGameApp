using Microsoft.Maui.Controls.Shapes;
using TriviaGameApp.Models;

namespace TriviaGameApp.Views;

public static class GameState
{
    public static List<Player>? Players { get; set; }
    public static int TotalQuestionsPerPlayer { get; set; } = 10;
    public static int TimeLimitInSeconds { get; set; } = 10;
}

public partial class PlayersView : ContentPage
{
    private List<Entry> _playerNameEntries = new();

    public PlayersView()
    {
        InitializeComponent();
    }

    private void OnPlayerCountChanged(object sender, EventArgs e)
    {
        PlayerNamesContainer.Children.Clear();
        _playerNameEntries.Clear();

        int selectedCount = (int)PlayerCountPicker.SelectedItem;

        for (int i = 1; i <= selectedCount; i++)
        {
            var containerGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                Margin = new Thickness(0, 0, 0, 10)
            };

            var entry = new Entry
            {
                Placeholder = $"Player {i} Name (プレイヤー {i} 名前)",
                HorizontalOptions = LayoutOptions.Fill,
                Style = new Style(typeof(Entry))
                {
                    Setters =
                    {
                        new Setter { Property = Entry.FontFamilyProperty, Value = "Yu Mincho, 'MS Mincho', serif" },
                        new Setter { Property = Entry.BackgroundColorProperty, Value = Colors.Transparent },
                        new Setter { Property = Entry.TextColorProperty, Value = Application.Current.RequestedTheme == AppTheme.Dark ? 
                            (Color)Application.Current.Resources["Primary_Dark"] : 
                            (Color)Application.Current.Resources["Primary"] },
                        new Setter { Property = Entry.FontSizeProperty, Value = 16.0 },
                        new Setter { Property = Entry.PlaceholderColorProperty, Value = Colors.Gray },
                        new Setter { Property = Entry.MinimumWidthRequestProperty, Value = 200.0 },
                        new Setter { Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                        new Setter { Property = Entry.MarginProperty, Value = new Thickness(0, 5) }
                    }
                }
            };

            var border = new Border
            {
                Content = entry,
                Stroke = Application.Current.RequestedTheme == AppTheme.Dark ? 
                    (Color)Application.Current.Resources["Primary_Dark"] : 
                    (Color)Application.Current.Resources["Primary"],
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 0 },
                Padding = new Thickness(10, 5),
                HorizontalOptions = LayoutOptions.Center
            };

            containerGrid.Add(border);
            _playerNameEntries.Add(entry);
            PlayerNamesContainer.Children.Add(containerGrid);

            if (i < selectedCount)
            {
                var line = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 100,
                    Y2 = 0,
                    Stroke = Application.Current.RequestedTheme == AppTheme.Dark ? 
                        (Color)Application.Current.Resources["Primary_Dark"] : 
                        (Color)Application.Current.Resources["Primary"],
                    StrokeThickness = 1,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10)
                };
                PlayerNamesContainer.Children.Add(line);
            }
        }
    }

    private async void OnStartGameClicked(object sender, EventArgs e)
    {
        if (!ValidatePlayerNames())
        {
            await DisplayAlert("入力エラー / Input Error", 
                "Please enter names for all players\nすべてのプレイヤーの名前を入力してください", 
                "OK");
            return;
        }

        int selectedCount = (int)PlayerCountPicker.SelectedItem;
        var players = new List<Player>();
        
        for (int i = 0; i < selectedCount; i++)
        {
            players.Add(new Player
            {
                Name = string.IsNullOrWhiteSpace(_playerNameEntries[i].Text) ? 
                    $"Player {i+1}" : _playerNameEntries[i].Text,
                Score = 0
            });
        }

        GameState.Players = players;
        GameState.TotalQuestionsPerPlayer = 10;
        await Shell.Current.GoToAsync($"//GamePage");
    }

    private bool ValidatePlayerNames()
    {
        return _playerNameEntries.All(entry => !string.IsNullOrWhiteSpace(entry.Text));
    }
}