using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TriviaGameApp.Models;
using TriviaGameApp.Services;

namespace TriviaGameApp.Views
{
    public partial class GamePage : ContentPage
    {
        private TriviaService _triviaService = new();
        
        private Dictionary<int, List<TriviaQuestion>> _playerQuestions = new();
        private int _currentPlayerIndex = 0;
        private int _currentQuestionIndex = 0;

        private DateTime _gameStartTime;

        private int _timeLeft;
        private bool _timerRunning = false;

        public GamePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _gameStartTime = DateTime.Now;

            if (GameState.Players == null || GameState.Players.Count == 0)
            {
                await DisplayAlert("Error", "No players found. Returning to start.", "OK");
                await Shell.Current.GoToAsync("//StartPage");
                return;
            }

            for (int i = 0; i < GameState.Players.Count; i++)
            {
                List<TriviaQuestion> fetched = await _triviaService.FetchTriviaAsync();
                _playerQuestions[i] = fetched;
            }

            _currentPlayerIndex = 0;
            _currentQuestionIndex = 0;
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (GameState.Players == null) return;

            var currentPlayer = GameState.Players[_currentPlayerIndex];
            var questions = _playerQuestions[_currentPlayerIndex];
            if (_currentQuestionIndex >= questions.Count)
            {
                _currentQuestionIndex = 0;
                _currentPlayerIndex++;
                
                if (_currentPlayerIndex >= GameState.Players.Count)
                {
                    EndGame();
                    return;
                }
            }
            
            PlayerLabel.Text = $"{currentPlayer.Name}'s Turn (Score: {currentPlayer.Score})";

            var question = questions[_currentQuestionIndex];
            QuestionLabel.Text = question.Question;

            AnswersContainer.Children.Clear();
            foreach (var answer in question.AllAnswers)
            {
                var answerButton = new Button
                {
                    Text = answer,
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                answerButton.Clicked += (s, e) => OnAnswerSelected(answer, question);
                AnswersContainer.Children.Add(answerButton);
            }

            StartTimer();
        }

        private void OnAnswerSelected(string chosenAnswer, TriviaQuestion question)
        {

            bool isCorrect = chosenAnswer == question.CorrectAnswer;

            if (isCorrect)
            {
                int points = GetPointsForDifficulty(question.Difficulty);

                var currentPlayer = GameState.Players?[_currentPlayerIndex];
                if (currentPlayer != null)
                {
                    currentPlayer.Score += points;
                }
                DisplayAlert("Correct!", $"You earned {points} point(s).", "OK");
            }
            else
            {
                DisplayAlert("Incorrect!", "Better luck next time.", "OK");
            }
            
            _currentQuestionIndex++;
            LoadQuestion();
        }

        private int GetPointsForDifficulty(string? difficulty)
        {
            switch (difficulty?.ToLower())
            {
                case "easy": return 1;
                case "medium": return 2;
                case "hard": return 3;
                default: return 1; 
            }
        }

        private void StartTimer()
        {
            _timeLeft = GameState.TimeLimitInSeconds;
            _timerRunning = true;
            UpdateTimerLabel();

            Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (!_timerRunning) return false;
                
                _timeLeft--;
                UpdateTimerLabel();

                TimerLabel.RotateTo(10, 200, Easing.Linear)
                          .ContinueWith((t) => TimerLabel.RotateTo(-10, 200, Easing.Linear))
                          .ContinueWith((t) => TimerLabel.RotateTo(0, 200, Easing.Linear));

                if (_timeLeft <= 0)
                {
                    _timerRunning = false;
                    DisplayAlert("Time's Up!", "You ran out of time.", "OK");
                    _currentQuestionIndex++;
                    LoadQuestion();
                    return false;
                }

                return true;
            });
        }

        private void UpdateTimerLabel()
        {
            TimerLabel.Text = $"Time Left: {_timeLeft}";
        }

        private async void OnSkipClicked(object sender, EventArgs e)
        {
            _timerRunning = false;
            _currentQuestionIndex++;
            LoadQuestion();
        }

        private async void EndGame()
        {
        {
            // Stop the clock
            var totalTime = DateTime.Now - _gameStartTime;
            int totalSeconds = (int)totalTime.TotalSeconds;

            // Load existing leaderboard from file
            var leaderboard = LeaderboardFileService.LoadLeaderboard();

            string categoryText = CategoryHelper.GetCategoryText(TriviaSettings.Category);

            // Suppose each player answered X questions; 
            // if each has 10 in your logic, or you can track how many they actually answered.
            // For simplicity, let's say each player's question count was set or known:
            int questionsPerPlayer = _playerQuestions.Count > 0 
                ? _playerQuestions[0].Count 
                : 10; // or read from your logic


            string dateOfGame = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); 


            // Create an entry for each player
            foreach (var player in GameState.Players)
            {
                var entry = new LeaderboardEntry
                {
                    Name = player.Name,
                    Category = categoryText,     // or your chosen category
                    Questions = questionsPerPlayer,         // or however many answered
                    TimeInSeconds = totalSeconds,           // total match time for all players
                    TotalPoints = player.Score,
                    DateOfGame = dateOfGame
                };
                leaderboard.Add(entry);
            }

            // Save updated leaderboard
            LeaderboardFileService.SaveLeaderboard(leaderboard);

            // Show final scores or do something else
            string resultMessage = "Final Scores:\n\n";
            foreach (var p in GameState.Players)
            {
                resultMessage += $"{p.Name}: {p.Score}\n";
            }
            await DisplayAlert("Game Over!", resultMessage, "OK");

            // Optionally navigate to Leaderboard page
            await Shell.Current.GoToAsync("LeaderboardPage");
        }
        }
    }
}
