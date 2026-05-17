<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Recipe.aspx.cs" Inherits="Cooking_Website.Recipe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/recipe.css" />
    <script type="text/javascript">
        var RecipeConfig = {
            defaultServings:    <%# CurrentRecipe != null ? CurrentRecipe.Servings : 1 %>,
            hiddenDifficultyId: "<%= HiddenDifficulty.ClientID %>",
            hiddenCuisinesId:   "<%= HiddenCuisines.ClientID %>"
        };
    </script>
    <script src="/js/recipe.js" defer></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <main class="recipe-container">

        <!-- ── Back Button ── -->
        <a class="back-btn" id="back-btn" href="/Recipes.aspx">&#8592; Back to Recipes
        </a>

        <!-- ── Hero Image (only if recipe has an image) ── -->
        <asp:Panel ID="HeroImagePanel" runat="server" CssClass="recipe-hero-image">
            <asp:Image ID="HeroImage" runat="server" CssClass="recipe-hero-img" />
        </asp:Panel>

        <!-- ── Header ── -->
        <div class="recipe-header">

            <div class="recipe-tags">
                <span class="recipe-tag"><%# CurrentRecipe.Category %></span>
                <span class="recipe-tag"><%# CurrentRecipe.Cuisine %></span>
                <span class="recipe-difficulty <%# CurrentRecipe.Difficulty.ToLower() %>">
                    <%# CurrentRecipe.Difficulty %>
                </span>
                <asp:Panel ID="ForYouPanel" runat="server" CssClass="for-you-badge">
                    For You
                </asp:Panel>
            </div>

            <h1><%# CurrentRecipe.Title %></h1>
            <p class="recipe-description"><%# CurrentRecipe.Description %></p>

            <!-- Stats row -->
            <div class="recipe-stats">
                <div class="stat">
                    <span class="stat-label">Prep</span>
                    <span class="stat-value"><%# CurrentRecipe.PrepTime %> min</span>
                </div>
                <div class="stat-divider"></div>
                <div class="stat">
                    <span class="stat-label">Cook</span>
                    <span class="stat-value"><%# CurrentRecipe.CookTime %> min</span>
                </div>
                <div class="stat-divider"></div>
                <div class="stat">
                    <span class="stat-label">Total</span>
                    <span class="stat-value"><%# CurrentRecipe.PrepTime + CurrentRecipe.CookTime %> min</span>
                </div>
                <div class="stat-divider"></div>
                <div class="stat">
                    <span class="stat-label">Servings</span>
                    <div class="servings-scaler">
                        <button class="scaler-btn" id="scaler-minus" type="button" aria-label="Decrease servings">&#8722;</button>
                        <span class="scaler-value" id="scaler-value"><%# CurrentRecipe.Servings %></span>
                        <button class="scaler-btn" id="scaler-plus" type="button" aria-label="Increase servings">&#43;</button>
                    </div>
                </div>
            </div>

        </div>

        <!-- ── Ingredients ── -->
        <section class="recipe-section">
            <h2 class="section-title">Ingredients</h2>
            <p class="section-note">Click an ingredient to mark it as gathered.</p>
            <ul class="ingredients-list" id="ingredients-list">
                <asp:Repeater ID="IngredientsRepeater" runat="server">
                    <itemtemplate>
                        <li class="ingredient-item">
                            <span class="ingredient-check" aria-hidden="true">&#10003;</span>
                            <span class="ingredient-text">
                                <span class="ingredient-quantity"
                                    data-original='<%# Eval("Quantity") %>'><%# Eval("Quantity") %></span>
                                <%# RenderUnit(Eval("Unit")) %>
                                <span class="ingredient-name"><%# Eval("Name") %></span>
                            </span>
                        </li>
                    </itemtemplate>
                </asp:Repeater>
            </ul>
            <button class="clear-checks-btn" id="clear-checks-btn" type="button">Clear all</button>
        </section>

        <!-- ── Method ── -->
        <section class="recipe-section">
            <h2 class="section-title">Method</h2>
            <ol class="steps-list">
                <asp:Repeater ID="StepsRepeater" runat="server">
                    <itemtemplate>
                        <li>
                            <div class="step-body"><%# Container.DataItem %></div>
                        </li>
                    </itemtemplate>
                </asp:Repeater>
            </ol>
        </section>

        <!-- ── Tips (only if recipe has tips) ── -->
        <asp:Panel ID="TipsPanel" runat="server" CssClass="recipe-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <asp:Repeater ID="TipsRepeater" runat="server">
                    <itemtemplate>
                        <li>
                            <div class="tip-body"><%# Container.DataItem %></div>
                        </li>
                    </itemtemplate>
                </asp:Repeater>
            </ol>
        </asp:Panel>

        <!-- Hidden fields for For You badge logic -->
        <asp:HiddenField ID="HiddenDifficulty" runat="server" Value="" />
        <asp:HiddenField ID="HiddenCuisines" runat="server" Value="" />

    </main>
</asp:Content>
