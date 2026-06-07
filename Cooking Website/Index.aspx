<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Cooking_Website.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="/css/index.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

    <!-- ── Hero ── -->
    <section class="hero">
        <div class="hero-text">
            <%-- Personalise the greeting with the username when a user is logged in --%>
        <h2 class="hero-eyebrow">Welcome<% if (Session["Id"] != null) Response.Write($", {Session["Username"]}"); %>!</h2>
            <h1>FreshBite</h1>
            <p>Discover easy recipes and cooking tips that make every meal better.</p>
            <a href="Recipes.aspx" class="btn">Explore Recipes</a>
        </div>
        <div class="hero-img">
            <img src="https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=900" alt="Food" />
        </div>
    </section>

    <!-- ── Features ── -->
    <section class="features">
        <div class="feature-card">
            <span class="feature-icon">&#9997;</span>
            <h3>Cooking Guides</h3>
            <p>In-depth guides for every ingredient. Learn the techniques, not just the recipe.</p>
            <% if (Session["Id"] != null) { %>
            <a href="/Guides/MeatGuide.aspx" class="feature-link">Browse guides &#8594;</a>
            <% } else { %>
            <p class="feature-login-notice"><strong>&#128274; <a href="/Login.aspx">Log in</a> to access the guides.</strong></p>
            <% } %>
        </div>
        <div class="feature-card">
            <span class="feature-icon">&#127859;</span>
            <h3>Recipes</h3>
            <p>A curated collection filtered to your skill level and favourite cuisines.</p>
            <a href="/Recipes.aspx" class="feature-link">View recipes &#8594;</a>
        </div>
        <div class="feature-card">
            <span class="feature-icon">&#9201;</span>
            <h3>Kitchen Tools</h3>
            <p>A units calculator for any measurement in any recipe, and a distraction-free cooking timer.</p>
            <a href="/Utilities/UnitsCalculator.aspx" class="feature-link">Calculator &#8594;</a>
            <a href="/Utilities/Timer.aspx" class="feature-link">Timer &#8594;</a>
        </div>
    </section>

    <!-- ── Call to Action (guests only) — hidden for logged-in users ── -->
    <% if (Session["Id"] == null) { %>
    <section class="cta-strip">
        <div class="cta-text">
            <h2>Get personalised suggestions</h2>
            <p>Create a free account and FreshBite will match recipes to your skill level and favourite cuisines automatically.</p>
        </div>
        <div class="cta-actions">
            <a href="/Register.aspx" class="btn">Create account</a>
            <a href="/Login.aspx" class="cta-login">Sign in</a>
        </div>
    </section>
    <% } %>

</asp:Content>