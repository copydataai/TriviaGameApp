using Microsoft.Maui.Controls;
using TriviaGameApp.Views;
namespace TriviaGameApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("PlayersView", typeof(PlayersView));
            Routing.RegisterRoute("LeaderboardPage", typeof(LeaderboardPage));
            Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
        }
    }
}
