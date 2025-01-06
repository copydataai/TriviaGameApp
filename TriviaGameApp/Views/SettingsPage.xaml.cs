using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using TriviaGameApp.Services;

namespace TriviaGameApp.Views;
public partial class SettingsPage : ContentPage
{

    private List<CategoryOption> _categories = CategoryHelper.AllCategories;    
    private List<SimpleOption> _difficulties = DifficultyHelper.AllDifficulties;
    private List<SimpleOption> _types = TypeHelper.AllTypes;
    
    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CategoryPicker.ItemsSource = CategoryHelper.AllCategories;
        DifficultyPicker.ItemsSource = DifficultyHelper.AllDifficulties;
        TypePicker.ItemsSource = TypeHelper.AllTypes;

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

