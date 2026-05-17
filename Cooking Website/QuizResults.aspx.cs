using Cooking_Website.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Schema;

namespace Cooking_Website
{
    public partial class QuizResults : System.Web.UI.Page
    {
        protected List<QuizQuestion> questions;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["QuizResults"] == null)
            {
                Response.Redirect("Quiz.aspx");
                return;
            }

            questions = (List<QuizQuestion>)Session["Questions"];

            double score = 0;
            int correctCount = 0, wrongCount = 0;
            var quizResults = (Dictionary<int, bool>)Session["QuizResults"];
            // Calculate score
            foreach (var result in quizResults)
                if (result.Value)
                {
                    correctCount++;
                    score += 100.0 / quizResults.Count;
                }
                else
                {
                    wrongCount++;
                }

            // Calculate progress bar
            double circumference = 2 * Math.PI * double.Parse(progressFill.Attributes["r"]);
            double offset = circumference - (circumference * (score / 100));
            progressFill.Attributes["style"] = $"stroke-dashoffset: {offset};";
            progressText.InnerText = $"{(int)score}%";

            // Score label
            if (score > 75)
            {
                scoreLabel.InnerText = "Master Chef!";
                progressFill.Attributes["style"] += $"stroke: #2e7d32;";
            }
            else if (score > 50)
            {
                scoreLabel.InnerText = "Good job!";
                progressFill.Attributes["style"] += $"stroke: #f5a623;";
            }
            else if (score > 25)
            {
                scoreLabel.InnerText = "Could be better...";
                progressFill.Attributes["style"] += $"stroke: #ef6c00;";
            }
            else
            {
                scoreLabel.InnerText = "Try again";
                progressFill.Attributes["style"] += $"stroke: #c62828;";
            }

            correctSpan.InnerText = correctCount.ToString();
            wrongSpan.InnerText = wrongCount.ToString();
            timeSpan.InnerText = (string)Session["QuizTimer"];
        }
    }
}