using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web; // For HTML decoding
using TriviaGameApp.Models;

namespace TriviaGameApp.Services
{
    public class TriviaService
    {
        private const string BaseUrl = "https://opentdb.com/api.php";

        private readonly HttpClient _httpClient;

        public TriviaService()
        {
            // You can re-use a single HttpClient instance if you want.
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Fetches 10 (or specified amount of) trivia questions from the Open Trivia DB.
        /// By default, it uses the endpoint: https://opentdb.com/api.php?amount=10
        /// </summary>
        /// <param name="amount">Number of questions to retrieve</param>
        /// <returns>A list of <see cref="TriviaQuestion"/> objects with cleaned question data.</returns>
        public async Task<List<TriviaQuestion>> FetchTriviaAsync()
        {
            try
            {
                int amount = TriviaSettings.NumberOfQuestions;
                string category = TriviaSettings.Category;
                string difficulty = TriviaSettings.Difficulty;
                string type = TriviaSettings.TriviaType;
                // Build the request URL with query params
                string requestUrl = $"{BaseUrl}?amount={amount}";

                // Get JSON as string

                if (category != "any")
                {
                    requestUrl += $"&category={category}";
                }

                if(difficulty != "any")
                {
                    requestUrl += $"&difficulty={difficulty}";
                }

                if (type != "any")
                {
                    requestUrl += $"&type={type}";
                }

                string json = await _httpClient.GetStringAsync(requestUrl);
                // Deserialize into our OpenTDBResponse structure
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var apiResponse = JsonSerializer.Deserialize<OpenTDBResponse>(json, options);

                if (apiResponse == null || apiResponse.Results == null)
                {
                    return new List<TriviaQuestion>();
                }

                // Convert raw OpenTDBQuestions into our clean TriviaQuestion objects
                return ConvertToTriviaQuestions(apiResponse.Results);
            }
            catch (Exception ex)
            {
                // In production, log or handle the exception properly.
                Console.WriteLine($"Error fetching trivia: {ex.Message}");
                return new List<TriviaQuestion>();
            }
        }

        /// <summary>
        /// Converts a list of OpenTDBQuestion objects to a list of TriviaQuestion objects.
        /// This also decodes HTML entities and merges correct/incorrect answers.
        /// </summary>
        private List<TriviaQuestion> ConvertToTriviaQuestions(List<OpenTDBQuestion> openTDBQuestions)
        {
            var result = new List<TriviaQuestion>();
            // TODO: Check that the questions are enough or make just one fetch per all 

            foreach (var q in openTDBQuestions)
            {
                // Decode HTML entities in question/answers
                string questionText = HttpUtility.HtmlDecode(q.Question ?? string.Empty);
                string correctText = HttpUtility.HtmlDecode(q.CorrectAnswer ?? string.Empty);

                // Build final TriviaQuestion object
                var triviaQuestion = new TriviaQuestion
                {
                    Category = HttpUtility.HtmlDecode(q.Category ?? string.Empty),
                    Difficulty = q.Difficulty,
                    Question = questionText,
                    CorrectAnswer = correctText,
                    AllAnswers = new List<string>()
                };

                // Add correct answer
                triviaQuestion.AllAnswers.Add(correctText);

                // Add incorrect answers (after decoding)
                if (q.IncorrectAnswers != null)
                {
                    foreach (var inc in q.IncorrectAnswers)
                    {
                        triviaQuestion.AllAnswers.Add(HttpUtility.HtmlDecode(inc));
                    }
                }

                // Optional: Shuffle the answers if desired
                // ShuffleAnswers(triviaQuestion.AllAnswers);

                result.Add(triviaQuestion);
            }

            return result;
        }

        /// <summary>
        /// Example method to shuffle a list of strings in-place. 
        /// You can call this in ConvertToTriviaQuestions if you want random answer orders.
        /// </summary>
        /// <param name="answers"></param>
        private void ShuffleAnswers(List<string> answers)
        {
            Random rng = new();
            int n = answers.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (answers[k], answers[n]) = (answers[n], answers[k]);
            }
        }
    }
}
