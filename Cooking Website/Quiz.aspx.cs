using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.ModelBinding;
using Cooking_Website.Helpers;
using System.Diagnostics;

namespace Cooking_Website
{
    public partial class Quiz : System.Web.UI.Page
    {
        protected List<QuizQuestion> questions;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Load, shuffle and store in session
                questions = QuizHelper.LoadQuizQuestions()
                    .OrderBy(q => Guid.NewGuid())
                    .Select(q => {
                        q.Answers = q.Answers.OrderBy(a => Guid.NewGuid()).ToList();
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

        protected void startBtn_Click(object sender, EventArgs e)
        {
            Session["QuizStart"] = DateTime.Now; // store start time
            // Note: Since the start button is server-side, the page reloads after the button is clicked.
            //       That means the state of the quiz (start) - show quiz questions, hide start button - is forgotten
            //       That is why we use restoreQuizState - to signal the client the page reloaded
            restoreQuizState.Value = "true";
        }

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
            Response.Redirect("QuizResults.aspx");
        }
    }
}