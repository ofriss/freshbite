<%@ Page Language="C#" MasterPageFile="~/Guides/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="SaucesGuide.aspx.cs" Inherits="Cooking_Website.SaucesGuide" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    Sauces Guide
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Sauces</h1>
            <p class="guide-intro">
                A good sauce can elevate the simplest ingredient into something memorable.
            Most sauces are built on a handful of core techniques — reduction, emulsification,
            and thickening — and once you understand these, you can make almost any sauce
            without a recipe.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Reduction is your most powerful tool.</span>
                        Simmering a sauce down concentrates flavour and thickens the texture
                    naturally — no starch needed. Keep the heat medium and be patient.
                    Taste frequently as it reduces, as salt intensifies along with everything else.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Butter finishing (monter au beurre) transforms a sauce.</span>
                        Whisking cold butter into a sauce off the heat emulsifies it into a
                    glossy, rich, velvety consistency. Add the butter in small pieces and
                    keep the sauce moving. Don't let it boil after adding butter or it will split.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Season a sauce at the very end.</span>
                        Salt concentrates as a sauce reduces. Season lightly during cooking and
                    do the final adjustment just before serving, when the flavour is fully developed.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Emulsified sauces need temperature control.</span>
                        Hollandaise and béarnaise break if they get too hot — keep them between
                    60–65°C. Too cool and they won't emulsify; too hot and the eggs scramble.
                    A double boiler gives you the most control.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">A roux must be cooked properly before adding liquid.</span>
                        For béchamel and velouté, cook the flour and butter together for at least
                    1–2 minutes before adding liquid. Under-cooked roux tastes of raw flour.
                    Add the liquid gradually and whisk constantly to avoid lumps.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Acid balances richness.</span>
                        A few drops of lemon juice or a splash of vinegar added to a rich sauce
                    at the end cuts through fat and brings the flavours into focus.
                    It should be subtle — you shouldn't taste it distinctly.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Strain for a professional result.</span>
                        Passing a sauce through a fine mesh sieve removes solids and produces
                    a smooth, restaurant-quality texture. Essential for pan sauces, gravies,
                    and anything you want to look polished.
                    </div>
                </li>
            </ol>
        </section>

    </main>
</asp:Content>
