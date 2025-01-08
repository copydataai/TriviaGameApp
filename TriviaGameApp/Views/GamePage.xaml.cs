using Microsoft.Maui.Controls;
using System.Collections.Generic;
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

        private bool _gameEnded = false;  // guard for EndGame()

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

            List<TriviaQuestion> fetched = await _triviaService.FetchTriviaAsync(GameState.Players.Count);
            int questionsPerPlayer = fetched.Count / GameState.Players.Count;
            int remainingQuestions = fetched.Count % GameState.Players.Count;

            for (int i = 0; i < GameState.Players.Count; i++)
            {
                int startIndex = i * questionsPerPlayer;
                int count = questionsPerPlayer + (i < remainingQuestions ? 1 : 0);
                _playerQuestions[i] = fetched.GetRange(startIndex, count);

                // Debug check:
                System.Diagnostics.Debug.WriteLine(
                $"DEBUG: Player '{GameState.Players[i].Name}' got {_playerQuestions[i].Count} questions."
                );
            }

            // Reset indexes
            _currentPlayerIndex = 0;
            _currentQuestionIndex = 0;
            _gameEnded = false;

            // Load first question
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            // If we've already ended, do nothing
            if (_gameEnded) return;

            // 1) Are we out of players?
            if (_currentPlayerIndex >= GameState.Players.Count)
            {
                EndGame();
                return;
            }

            var currentPlayer = GameState.Players[_currentPlayerIndex];
            var questions = _playerQuestions[_currentPlayerIndex];

            // 2) Are we out of questions for this player?
            if (_currentQuestionIndex >= questions.Count)
            {
                // Move on to next player
                _currentPlayerIndex++;
                _currentQuestionIndex = 0;

                // If that puts us out of players, end game
                if (_currentPlayerIndex >= GameState.Players.Count)
                {
                    EndGame();
                    return;
                }

                // Otherwise load the next player’s first question
                LoadQuestion();
                return;
            }

            // 3) We have a valid question
            PlayerLabel.Text = $"{currentPlayer.Name}'s Turn (Score: {currentPlayer.Score})";
            
            var question = questions[_currentQuestionIndex];
            QuestionLabel.Text = question.Question;

            // 4) Create answer buttons
            AnswersContainer.Children.Clear();
            foreach (var answer in question.AllAnswers)
            {
                var answerButton = new Button
                {
                    Text = answer,
                    Style = (Style)Resources["JapaneseButtonStyle"]
                };
                answerButton.Clicked += (s, e) => OnAnswerSelected(answer, question);
                AnswersContainer.Children.Add(answerButton);
            }

            // 5) Start Timer
            StartTimer();
        }

        private void OnAnswerSelected(string chosenAnswer, TriviaQuestion question)
        {
            // Stop timer
            _timerRunning = false;

            // Disable answer buttons
            foreach (var view in AnswersContainer.Children)
            {
                if (view is Button btn)
                    btn.IsEnabled = false;
            }

            // Evaluate answer
            bool isCorrect = chosenAnswer == question.CorrectAnswer;
            if (isCorrect)
            {
                int points = GetPointsForDifficulty(question.Difficulty);
                GameState.Players[_currentPlayerIndex].Score += points;
                DisplayAlert("Correct!", $"You earned {points} points.", "OK");
            }
            else
            {
                DisplayAlert("Incorrect!", "Better luck next time.", "OK");
            }

            // Move to next question
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

                // Animate a little “shake”
                TimerLabel.RotateTo(10, 200, Easing.Linear)
                          .ContinueWith((t) => TimerLabel.RotateTo(-10, 200, Easing.Linear))
                          .ContinueWith((t) => TimerLabel.RotateTo(0, 200, Easing.Linear));

                if (_timeLeft <= 0)
                {
                    _timerRunning = false;
                    DisplayAlert("Time's Up!", "You ran out of time.", "OK");

                    // Disable buttons
                    foreach (var view in AnswersContainer.Children)
                    {
                        if (view is Button btn)
                            btn.IsEnabled = false;
                    }

                    // Move on
                    _currentQuestionIndex++;
                    LoadQuestion();
                    return false; // stop timer
                }

                return true; // keep ticking
            });
        }

        private void UpdateTimerLabel()
        {
            TimerLabel.Text = $"Time Left: {_timeLeft}";
        }

        private async void OnSkipClicked(object sender, EventArgs e)
        {
            // Stop timer
            _timerRunning = false;

            // Disable buttons
            foreach (var view in AnswersContainer.Children)
            {
                if (view is Button btn)
                    btn.IsEnabled = false;
            }

            // Move on
            _currentQuestionIndex++;
            LoadQuestion();
        }

        private async void EndGame()
        {
            if (_gameEnded) return;
            _gameEnded = true;

            // Stop the clock
            var totalTime = DateTime.Now - _gameStartTime;
            int totalSeconds = (int)totalTime.TotalSeconds;

            // Load existing leaderboard
            var leaderboard = LeaderboardFileService.LoadLeaderboard();
            string categoryText = CategoryHelper.GetCategoryText(TriviaSettings.Category);
            string difficultyText = TriviaSettings.Difficulty;

            // Example: if you expect 10 Q's per player
            int questionsPerPlayer = _playerQuestions.Count > 0 
                ? _playerQuestions[0].Count
                : 10;

            string dateOfGame = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            // Add each player's score exactly once
            foreach (var player in GameState.Players)
            {
                var entry = new LeaderboardEntry
                {
                    Name = player.Name,
                    Category = categoryText,
                    Questions = questionsPerPlayer,
                    TimeInSeconds = totalSeconds,
                    Difficulty = difficultyText,
                    TotalPoints = player.Score,
                    DateOfGame = dateOfGame
                };
                leaderboard.Add(entry);
            }

            LeaderboardFileService.SaveLeaderboard(leaderboard);

            // Show final scores
            string resultMessage = "Final Scores:\n\n";
            foreach (var p in GameState.Players)
            {
                resultMessage += $"{p.Name}: {p.Score}\n";
            }
            await DisplayAlert("Game Over!", resultMessage, "OK");

            // Navigate to Leaderboard
            await Shell.Current.GoToAsync("LeaderboardPage");
        }
    }
}
