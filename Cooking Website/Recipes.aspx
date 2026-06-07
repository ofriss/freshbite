<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Recipes.aspx.cs" Inherits="Cooking_Website.Recipes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/recipes.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <main class="recipes-container">

        <!-- ── Header ── -->
        <div class="recipes-header">
            <h1>Recipes</h1>
            <p>Browse our collection.</p>
        </div>

        <!-- ── Recipe Grid ── -->
        <div class="recipe-grid" id="recipe-grid">
            <%= BuildRecipeCardsHtml() %>
        </div>

    </main>
</asp:Content>
