using System.Collections.Generic;

namespace TriviaGameApp.Models
{
    /// <summary>
    /// A cleaner representation of a single trivia question
    /// in your application, after processing the raw OpenTDB data.
    /// </summary>
    public class TriviaQuestion
    {
        public string? Category { get; set; }
        public string? Difficulty { get; set; }
        public string? Question { get; set; }
        public string? CorrectAnswer { get; set; }
        
        /// <summary>
        /// All possible answers (correct + incorrect), 
        /// possibly in a random order if you want to shuffle them.
        /// </summary>
        public List<string> AllAnswers { get; set; } = new();
    }
}
