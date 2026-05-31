using Cooking_Website.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Cooking_Website
{
    public partial class Quiz : System.Web.UI.Page
    {
        protected List<QuizQuestion> questions;

        // On first load: shuffles question and answer order, caches in session.
        // On postback: restores the same order from session so answers still map correctly.
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                questions = QuizHelper.LoadQuizQuestions()
                .OrderBy(q => Guid.NewGuid())
                .Select(q =>
                {
                    // Save the correct answer text before the shuffle moves it
                    string correctAnswerText = q.Answers[q.Correct];

                    // Shuffle
                    q.Answers = q.Answers.OrderBy(a => Guid.NewGuid()).ToList();

                    // Update Correct to wherever that text ended up after shuffling
                    q.Correct = q.Answers.IndexOf(correctAnswerText);

                    return q;
                })
                .ToList();
                Session["Questions"] = questions;
            }
            else
            {
                // Restore questions from session on postback
                questions = (List<QuizQuestion>)Session["Questions"];
            }
        }

        // Records quiz start time and signals the client to restore the visible quiz state after postback
        protected void startBtn_Click(object sender, EventArgs e)
        {
            Session["QuizStart"] = DateTime.Now; // store start time
            // Note: Since the start button is server-side, the page reloads after the button is clicked.
            //       That means the state of the quiz (start) - show quiz questions, hide start button - is forgotten
            //       That is why we use restoreQuizState - to signal the client the page reloaded
            restoreQuizState.Value = "true";
        }

        // Scores all answers, records elapsed time, then redirects to QuizResults
        protected void submitBtn_Click(object sender, EventArgs e)
        {
            Dictionary<int, bool> quizResults = new Dictionary<int, bool>();

            for (int i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                string selected = Request.Form[$"q{i + 1}"];
                quizResults[i + 1] = selected == question.Answers[question.Correct];
            }

            // Stop the timer
            DateTime start = (DateTime)Session["QuizStart"];
            Session["QuizTimer"] = (DateTime.Now - start).ToString(@"mm\:ss");
            Session["QuizResults"] = quizResults;
            Response.Redirect("/QuizResults.aspx");
        }
    }
}