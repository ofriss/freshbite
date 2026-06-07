<%@ Page Title="" Language="C#" MasterPageFile="~/Utilities/UtilitiesSidebar.Master" AutoEventWireup="true" CodeBehind="Timer.aspx.cs" Inherits="Cooking_Website.Timer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="/css/timer.css" />
    <script src="/js/timer.js" defer></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <main class="main-container">

    <div class="timer-card">

        <!-- Time entry: three plain number inputs for hours / minutes / seconds -->
        <div class="timer-inputs" id="timer-inputs">
            <label>H<input id="inp-hours"   type="number" min="0" max="99" value="0" /></label>
            <label>M<input id="inp-minutes" type="number" min="0" max="59" value="5" /></label>
            <label>S<input id="inp-seconds" type="number" min="0" max="59" value="0" /></label>
        </div>

        <!-- Countdown readout (and "Done!" when finished) -->
        <div class="timer-display" id="timer-display">00:05:00</div>

        <!-- Start/Pause toggle and Reset -->
        <div class="timer-buttons">
            <button class="timer-btn" id="btn-start" type="button">Start</button>
            <button class="timer-btn secondary" id="btn-reset" type="button">Reset</button>
        </div>

    </div>

</main>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">Timer</asp:Content>