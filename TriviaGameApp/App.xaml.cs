using TriviaGameApp.Services;


namespace TriviaGameApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            TriviaSettings.LoadSettings();

            ApplyTheme(TriviaSettings.AppThemeChoice);

            MainPage = new AppShell();
        }


        public static void ApplyTheme(string themeChoice)
        {
            switch (themeChoice)
            {
                case "Dark":
                    Current.UserAppTheme = AppTheme.Dark;
                    break;
                case "Light":
                    Current.UserAppTheme = AppTheme.Light;
                    break;
                default:
                    Current.UserAppTheme = AppTheme.Unspecified;
                    break;
            }
        }
    }

}
