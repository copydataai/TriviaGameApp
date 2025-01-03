using TriviaGameApp.Services;


namespace TriviaGameApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            TriviaSettings.LoadSettings();

            MainPage = new AppShell();
        }
    }
}
