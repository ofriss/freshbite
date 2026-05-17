<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="QuizResults.aspx.cs" Inherits="Cooking_Website.QuizResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/quiz_results.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="main-container">
        <div class="container">
            <div class="card score">
                <svg class="progress-ring" viewBox="0 0 100 100">
                    <circle class="progress-ring-bg" cx="50" cy="50" r="40" />
                    <circle runat="server" id="progressFill" class="progress-ring-fill" cx="50" cy="50" r="40" />
                    <text runat="server" id="progressText" x="50" y="50" text-anchor="middle" dominant-baseline="middle" class="progress-ring-text">80%</text>
                </svg>
                <span runat="server" id="scoreLabel" class="score-label">Good job!</span>
            </div>
            <div class="card correct">
                <span runat="server" id="correctSpan" class="card-number">12</span>
                <span class="card-label">Correct</span>
            </div>
            <div class="card wrong">
                <span runat="server" id="wrongSpan" class="card-number">3</span>
                <span class="card-label">Wrong</span>
            </div>
            <div class="card time">
                <span runat="server" id="timeSpan" class="card-number">4:02</span>
                <span class="card-label">Time</span>
            </div>
            <% foreach (var result in (Dictionary<int, bool>)Session["QuizResults"])
                {
                    var question = questions[result.Key - 1]; %>
            <div class="question-result-card">
                <span class="question-number">Question <%= result.Key %></span>
                <p class="question-text"><%= question.Question %></p>
                <span class="question-tag <%= result.Value ? "correct" : "wrong" %>-tag"><%= result.Value ? "Correct" : "Wrong" %></span>
            </div>
            <% } %>
        </div>
    </div>

</asp:Content>
