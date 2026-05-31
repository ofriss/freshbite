<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Quiz.aspx.cs" Inherits="Cooking_Website.Quiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/quiz.css" />
    <link rel="stylesheet" href="/css/form.css" />
    <script src="/js/quiz.js" defer></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="main-container">
        <div>
            <h1 style="text-align: center;">Welcome, <%= Session["Username"] %>!</h1>
            <p id="quizInfo" style="text-align: center;">Click this button to start the quiz</p>
        </div>

        <asp:Button runat="server" OnClick="startBtn_Click" CssClass="start-btn" Text="Start" />
        <%-- Server sets this to "true" after start; client reads it to restore the visible quiz state on postback --%>
        <input type="hidden" runat="server" id="restoreQuizState" value="false" />
        <!-- Quiz section -->
        <section id="quizSection">
            <% for (int i = 0; i < questions.Count; i++)
                {
                    var question = questions[i]; %>
            <div class="question-container">
                <p><%= question.Question %></p>
                <div class="answers-container">
                    <%-- Note: the answers list inside of the question is not shuffled --%>
                    <%-- As the answers are accessed by the answer itself, it doesn't matter --%>
                    <% foreach (var answer in question.Answers)
                        { %>
                    <label class="answer">
                        <input type="radio" name="q<%= i + 1 %>" value="<%= answer %>" />
                        <%= answer %>
                    </label>
                    <%} %>
                </div>
            </div>
            <% } %>
        </section>
        <asp:Button runat="server" OnClick="submitBtn_Click" OnClientClick="return onSubmit()" Text="Send" CssClass="submit-btn btn" />
        <span id="msg"></span>

        <script>
            <%-- Inline variables read by quiz.js before the defer'd script runs --%>
            const questionsCount = <%= questions.Count %>;
            const restore = document.getElementById("<%= restoreQuizState.ClientID %>").value;
        </script>
    </div>
</asp:Content>
