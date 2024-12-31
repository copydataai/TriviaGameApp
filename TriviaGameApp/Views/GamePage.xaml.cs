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
        
        // Suppose each player gets their own set of questions (10 each).
        // We'll store them in a dictionary: PlayerIndex -> List of TriviaQuestion
        private Dictionary<int, List<TriviaQuestion>> _playerQuestions = new();

        private int _currentPlayerIndex = 0;
        private int _currentQuestionIndex = 0;

        // Timer
        private int _timeLeft;
        private bool _timerRunning = false;

        public GamePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (GameState.Players == null || GameState.Players.Count == 0)
            {
                await DisplayAlert("Error", "No players found. Returning to start.", "OK");
                await Shell.Current.GoToAsync("//StartPage");
                return;
            }

            // For demonstration: fetch each player's questions
            // If you want round-robin from a single set, you can just fetch once.
            for (int i = 0; i < GameState.Players.Count; i++)
            {
                // Fetch N questions from the service (e.g., 10).
                List<TriviaQuestion> fetched = await _triviaService.FetchTriviaAsync(GameState.TotalQuestionsPerPlayer);
                _playerQuestions[i] = fetched;
            }

            // Start first player's turn
            _currentPlayerIndex = 0;
            _currentQuestionIndex = 0;
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (GameState.Players == null)
                return;

            // Check if the current player is out of questions
            var questions = _playerQuestions[_currentPlayerIndex];
            if (_currentQuestionIndex >= questions.Count)
            {
                // Move to next player
                _currentQuestionIndex = 0;
                _currentPlayerIndex++;
                
                if (_currentPlayerIndex >= GameState.Players.Count)
                {
                    // All players done - game over
                    EndGame();
                    return;
                }
            }
            
            // Display current player
            var currentPlayer = GameState.Players[_currentPlayerIndex];
            PlayerLabel.Text = $"{currentPlayer.Name}'s Turn (Score: {currentPlayer.Score})";

            // Get the question
            var question = questions[_currentQuestionIndex];
            QuestionLabel.Text = question.Question;

            // Populate answers
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

            // Start the timer for this question
            StartTimer();
        }

        private void OnAnswerSelected(string chosenAnswer, TriviaQuestion question)
        {
            // Stop timer
            _timerRunning = false;

            // Evaluate correctness
            bool isCorrect = chosenAnswer == question.CorrectAnswer;

            if (isCorrect)
            {
                // Award points (e.g. +1 for easy, +2 for medium, +3 for hard, etc.)
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
            
            // Move to next question (or next player if out of questions)
            _currentQuestionIndex++;
            LoadQuestion();
        }

        private int GetPointsForDifficulty(string? difficulty)
        {
            // Example scoring logic
            switch (difficulty?.ToLower())
            {
                case "easy": return 1;
                case "medium": return 2;
                case "hard": return 3;
                default: return 1; // default to easy
            }
        }

        private void StartTimer()
        {
            // For demonstration, let's read from GameState
            _timeLeft = GameState.TimeLimitInSeconds;
            _timerRunning = true;
            UpdateTimerLabel();

            // Optionally animate the TimerLabel each second
            Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (!_timerRunning) return false;
                
                _timeLeft--;
                UpdateTimerLabel();

                // Simple "ticking" animation example
                TimerLabel.RotateTo(10, 200, Easing.Linear)
                          .ContinueWith((t) => TimerLabel.RotateTo(-10, 200, Easing.Linear))
                          .ContinueWith((t) => TimerLabel.RotateTo(0, 200, Easing.Linear));

                if (_timeLeft <= 0)
                {
                    // Time up
                    _timerRunning = false;
                    DisplayAlert("Time's Up!", "You ran out of time.", "OK");
                    // Move on to the next question
                    _currentQuestionIndex++;
                    LoadQuestion();
                    return false;
                }

                // Return true to keep timer going
                return true;
            });
        }

        private void UpdateTimerLabel()
        {
            TimerLabel.Text = $"Time Left: {_timeLeft}";
        }

        private async void OnSkipClicked(object sender, EventArgs e)
        {
            // If skipping is allowed, stop timer and move to next question
            _timerRunning = false;
            _currentQuestionIndex++;
            LoadQuestion();
        }

        private async void EndGame()
        {
            // All players have finished their questions
            // Sort players by score or just show them in a final scoreboard
            if (GameState.Players != null)
            {
                GameState.Players.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));
            }

            string resultMessage = "Final Scores:\n\n";
            foreach (var p in GameState.Players ?? new List<Player>())
            {
                resultMessage += $"{p.Name}: {p.Score}\n";
            }

            await DisplayAlert("Game Over!", resultMessage, "OK");

            // Possibly navigate to a LeaderboardPage or return to StartPage
            await Shell.Current.GoToAsync("LeaderboardPage");
        }
    }
}
