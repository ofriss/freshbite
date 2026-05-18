<%@ Page Title="" Language="C#" MasterPageFile="~/Utilities/UtilitiesSidebar.Master" AutoEventWireup="true" CodeBehind="Timer.aspx.cs" Inherits="Cooking_Website.Timer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="/css/timer.css" />
    <script src="/js/timer.js" defer></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <main class="main-container">

    <div class="timer-card">

        <!-- Editable time display — click any segment to edit when idle -->
        <div class="timer-display" id="timer-display" title="Click to edit">
            <span class="time-seg" id="seg-hours"   data-max="99">00</span>
            <span class="time-colon">:</span>
            <span class="time-seg" id="seg-minutes" data-max="59">05</span>
            <span class="time-colon">:</span>
            <span class="time-seg" id="seg-seconds" data-max="59">00</span>
        </div>

        <!-- Hidden input that overlays a segment when editing -->
        <input class="seg-input" id="seg-input" type="number" min="0" autocomplete="off" />

        <!-- Single Start / Stop button -->
        <button class="timer-btn" id="timer-btn" type="button">Start</button>

        <!-- Thin progress bar at the bottom of the card -->
        <div class="progress-bar-track">
            <div class="progress-bar-fill" id="progress-fill"></div>
        </div>

    </div>

</main>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="server">Timer</asp:Content>