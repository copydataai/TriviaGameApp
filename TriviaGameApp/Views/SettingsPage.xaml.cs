using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using TriviaGameApp.Services;

namespace TriviaGameApp.Views
{
    public partial class SettingsPage : ContentPage
    {
        private List<CategoryOption> _categories = CategoryHelper.AllCategories;
        private List<SimpleOption> _difficulties = DifficultyHelper.AllDifficulties;
        private List<SimpleOption> _types = TypeHelper.AllTypes;

        // We'll define theme modes as a simple list of strings
        private List<string> _themeOptions = new()
        {
            "System",
            "Light",
            "Dark"
        };

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Category, difficulty, type pickers
            CategoryPicker.ItemsSource = _categories;
            DifficultyPicker.ItemsSource = _difficulties;
            TypePicker.ItemsSource = _types;

            // Theme picker
            ThemePicker.ItemsSource = _themeOptions;

            // Select current Category
            int catIndex = _categories.FindIndex(c => c.Value == TriviaSettings.Category);
            CategoryPicker.SelectedIndex = catIndex >= 0 ? catIndex : 0;

            // Difficulty
            int diffIndex = _difficulties.FindIndex(d => d.Value == TriviaSettings.Difficulty);
            DifficultyPicker.SelectedIndex = diffIndex >= 0 ? diffIndex : 0;

            // Type
            int typeIndex = _types.FindIndex(t => t.Value == TriviaSettings.TriviaType);
            TypePicker.SelectedIndex = typeIndex >= 0 ? typeIndex : 0;

            // Number of Questions
            QuestionStepper.Value = TriviaSettings.NumberOfQuestions;

            // Theme
            int themeIdx = _themeOptions.IndexOf(TriviaSettings.AppThemeChoice);
            ThemePicker.SelectedIndex = themeIdx >= 0 ? themeIdx : 0;
        }

        private void OnQuestionsStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            // If you want to do something each time stepper changes, do it here
        }

        private async void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            // Update from UI
            if (CategoryPicker.SelectedIndex >= 0)
                TriviaSettings.Category = ((CategoryOption)CategoryPicker.SelectedItem).Value;

            if (DifficultyPicker.SelectedIndex >= 0)
                TriviaSettings.Difficulty = ((SimpleOption)DifficultyPicker.SelectedItem).Value;

            if (TypePicker.SelectedIndex >= 0)
                TriviaSettings.TriviaType = ((SimpleOption)TypePicker.SelectedItem).Value;

            TriviaSettings.NumberOfQuestions = (int)QuestionStepper.Value;

            // Theme
            if (ThemePicker.SelectedIndex >= 0)
                TriviaSettings.AppThemeChoice = _themeOptions[ThemePicker.SelectedIndex];

            // Save to file or preferences
            TriviaSettings.SaveSettings();

            // Apply the chosen theme
            App.ApplyTheme(TriviaSettings.AppThemeChoice);

            await DisplayAlert("Settings Saved", "Your trivia settings have been updated.", "OK");

            await Shell.Current.GoToAsync("..");
        }
    }
}
