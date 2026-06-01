<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Recipes.aspx.cs" Inherits="Cooking_Website.Recipes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/recipes.css" />
    <%-- Pass the mangled ASP.NET control IDs to JS before the defer'd script loads --%>
    <script type="text/javascript">
        var RecipesConfig = {
            hiddenDifficultyId: "<%= HiddenDifficulty.ClientID %>",
            hiddenCuisinesId:   "<%= HiddenCuisines.ClientID %>"
        };
    </script>
    <script src="/js/recipes.js" defer></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <main class="recipes-container">

        <!-- ── Header ── -->
        <div class="recipes-header">
            <h1>Recipes</h1>
            <p>Browse our collection — filter by difficulty, category, or cuisine.</p>
        </div>

        <!-- ── Guest Banner ── -->
        <asp:Panel ID="GuestBanner" runat="server" CssClass="guest-banner">
            <span class="guest-banner-icon">&#9670;</span>
            <span class="guest-banner-text">
                <a href="/Login.aspx">Sign in</a> or <a href="/Register.aspx">create an account</a>
                to get personalised recipe suggestions based on your skill level and favourite cuisines.
            </span>
            <button class="guest-banner-close" id="guest-banner-close" type="button">&#10005;</button>
        </asp:Panel>

        <!-- ── Filters ── -->
        <div class="filter-bar">

            <!-- Difficulty (single select) -->
            <div class="filter-group">
                <span class="filter-label">Difficulty</span>
                <div class="filter-btns">
                    <button class="filter-btn" data-filter-type="difficulty" data-filter="All" type="button">All</button>
                    <button class="filter-btn" data-filter-type="difficulty" data-filter="Easy" type="button">Easy</button>
                    <button class="filter-btn" data-filter-type="difficulty" data-filter="Medium" type="button">Medium</button>
                    <button class="filter-btn" data-filter-type="difficulty" data-filter="Hard" type="button">Hard</button>
                </div>
            </div>

            <!-- Category (single select) -->
            <div class="filter-group">
                <span class="filter-label">Category</span>
                <div class="filter-btns">
                    <button class="filter-btn" data-filter-type="category" data-filter="All" type="button">All</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Meat" type="button">Meat</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Poultry" type="button">Poultry</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Fish &amp; Seafood" type="button">Fish &amp; Seafood</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Vegetables" type="button">Vegetables</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Eggs &amp; Dairy" type="button">Eggs &amp; Dairy</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Pasta &amp; Grains" type="button">Pasta &amp; Grains</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Bread &amp; Baking" type="button">Bread &amp; Baking</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Soups &amp; Stews" type="button">Soups &amp; Stews</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Sauces" type="button">Sauces</button>
                    <button class="filter-btn" data-filter-type="category" data-filter="Other" type="button">Other</button>
                </div>
            </div>

            <!-- Cuisine (multi-select) -->
            <div class="filter-group">
                <span class="filter-label">Cuisine</span>
                <div class="filter-btns">
                    <button class="filter-btn" data-filter-type="cuisine" data-filter="All" type="button">All</button>
                    <%= BuildCuisineButtonsHtml() %>
                </div>
            </div>

        </div>

        <!-- Hidden fields — user profile values resolved server-side, read by JS -->
        <asp:HiddenField ID="HiddenDifficulty" runat="server" Value="" />
        <asp:HiddenField ID="HiddenCuisines" runat="server" Value="" />

        <!-- ── Recipe Grid ── -->
        <div class="recipe-grid" id="recipe-grid">
            <%= BuildRecipeCardsHtml() %>
        </div>

        <div class="recipes-empty" id="recipes-empty" hidden>
            <p>No recipes match the selected filters.</p>
        </div>

    </main>
</asp:Content>
