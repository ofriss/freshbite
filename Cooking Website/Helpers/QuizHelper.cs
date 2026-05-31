using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Cooking_Website.Helpers
{
    // Model for a single quiz question as stored in quiz_questions.json
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        // Zero-based index into Answers pointing to the correct option
        public int Correct { get; set; }
    }

    // Loads quiz questions from JSON; caches parsed result in a static field after first load
    public class QuizHelper
    {
        private static List<QuizQuestion> questions;
        // Returns a fresh deep copy each time so callers can shuffle without affecting the cache
        public static List<QuizQuestion> LoadQuizQuestions()
        {
            if (questions == null)
            {
                var serializer = new JavaScriptSerializer();
                var path = HttpContext.Current.Server.MapPath("~/Data/quiz_questions.json");
                var json = File.ReadAllText(path);
                questions = serializer.Deserialize<List<QuizQuestion>>(json);
            }

            // Return a deep copy so shuffling in Quiz.aspx.cs never mutates the cached list
            return questions.Select(q => new QuizQuestion
            {
                Id = q.Id,
                Question = q.Question,
                Answers = new List<string>(q.Answers),
                Correct = q.Correct
            }).ToList();
        }
    }
}