using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using TriviaGameApp.Services;

namespace TriviaGameApp.Views
{
    public partial class SettingsPage : ContentPage
    {
        private List<CategoryOption> _categories = new()
        {
            new() { Value = "any", Text = "Any Category" },
            new() { Value = "9",   Text = "General Knowledge" },
            new() { Value = "10",  Text = "Entertainment: Books" },
            new() { Value = "11",  Text = "Entertainment: Film" },
            new() { Value = "12",  Text = "Entertainment: Music" },
            new() { Value = "13",  Text = "Entertainment: Musicals & Theatres" },
            new() { Value = "14",  Text = "Entertainment: Television" },
            new() { Value = "15",  Text = "Entertainment: Video Games" },
            new() { Value = "16",  Text = "Entertainment: Board Games" },
            new() { Value = "17",  Text = "Science & Nature" },
            new() { Value = "18",  Text = "Science: Computers" },
            new() { Value = "19",  Text = "Science: Mathematics" },
            new() { Value = "20",  Text = "Mythology" },
            new() { Value = "21",  Text = "Sports" },
            new() { Value = "22",  Text = "Geography" },
            new() { Value = "23",  Text = "History" },
            new() { Value = "24",  Text = "Politics" },
            new() { Value = "25",  Text = "Art" },
            new() { Value = "26",  Text = "Celebrities" },
            new() { Value = "27",  Text = "Animals" },
            new() { Value = "28",  Text = "Vehicles" },
            new() { Value = "29",  Text = "Entertainment: Comics" },
            new() { Value = "30",  Text = "Science: Gadgets" },
            new() { Value = "31",  Text = "Entertainment: Japanese Anime & Manga" },
            new() { Value = "32",  Text = "Entertainment: Cartoon & Animations" }
        };
        
        private List<SimpleOption> _difficulties = new()
        {
            new() { Value = "any",   Text = "Any Difficulty" },
            new() { Value = "easy",  Text = "Easy" },
            new() { Value = "medium",Text = "Medium" },
            new() { Value = "hard",  Text = "Hard" }
        };

        private List<SimpleOption> _types = new()
        {
            new() { Value = "any",      Text = "Any Type" },
            new() { Value = "multiple", Text = "Multiple Choice" },
            new() { Value = "boolean",  Text = "True / False" }
        };
        
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            CategoryPicker.ItemsSource = _categories;
            DifficultyPicker.ItemsSource = _difficulties;
            TypePicker.ItemsSource = _types;

            int catIndex = _categories.FindIndex(c => c.Value == TriviaSettings.Category);
            CategoryPicker.SelectedIndex = catIndex >= 0 ? catIndex : 0;

            int diffIndex = _difficulties.FindIndex(d => d.Value == TriviaSettings.Difficulty);
            DifficultyPicker.SelectedIndex = diffIndex >= 0 ? diffIndex : 0;

            int typeIndex = _types.FindIndex(t => t.Value == TriviaSettings.TriviaType);
            TypePicker.SelectedIndex = typeIndex >= 0 ? typeIndex : 0;

            QuestionStepper.Value = TriviaSettings.NumberOfQuestions;
        }

        private void OnQuestionsStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            // TODO: Update the label to show the new value
        }

        private async void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            // Update TriviaSettings from UI
            if (CategoryPicker.SelectedIndex >= 0)
                TriviaSettings.Category = ((CategoryOption)CategoryPicker.SelectedItem).Value;

            if (DifficultyPicker.SelectedIndex >= 0)
                TriviaSettings.Difficulty = ((SimpleOption)DifficultyPicker.SelectedItem).Value;

            if (TypePicker.SelectedIndex >= 0)
                TriviaSettings.TriviaType = ((SimpleOption)TypePicker.SelectedItem).Value;

            TriviaSettings.NumberOfQuestions = (int)QuestionStepper.Value;

            TriviaSettings.SaveSettings();

            await DisplayAlert("Settings Saved", "Your trivia settings have been updated.", "OK");

            await Shell.Current.GoToAsync("..");
        }
    }

    public class CategoryOption
    {
        public string Value { get; set; } = "any";
        public string Text { get; set; } = "Any Category";
        public override string ToString() => Text; 
    }

    public class SimpleOption
    {
        public string Value { get; set; } = "any";
        public string Text { get; set; } = "Any";
        public override string ToString() => Text; 
    }
}
