using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Cooking_Website.Helpers
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public int Correct { get; set; }
    }

    public class QuizHelper
    {
        private static List<QuizQuestion> questions;
        public static List<QuizQuestion> LoadQuizQuestions()
        {
            if (questions == null)
            {
                var serializer = new JavaScriptSerializer();
                var path = HttpContext.Current.Server.MapPath("~/Data/quiz_questions.json");
                var json = File.ReadAllText(path);
                questions = serializer.Deserialize<List<QuizQuestion>>(json);
            }
            
            return questions;
        }
    }
}