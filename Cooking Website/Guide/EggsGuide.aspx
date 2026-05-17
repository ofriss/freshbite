<%@ Page Language="C#" MasterPageFile="~/Guide/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="EggsGuide.aspx.cs" Inherits="Cooking_Website.EggsGuide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Eggs &amp; Dairy</h1>
            <p class="guide-intro">
                Eggs are one of the most versatile ingredients in cooking — they emulsify, leaven,
            bind, enrich, and coat. Dairy adds fat, flavour, and texture to everything from
            sauces to baked goods. Both are sensitive to heat and need gentle handling
            to get the best results.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Use room temperature eggs for baking.</span>
                        Cold eggs don't incorporate as smoothly into batters and can cause butter
                    to seize in creamed mixtures. Take them out 30 minutes before you start.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Low and slow for scrambled eggs.</span>
                        The best scrambled eggs are cooked over low heat with constant, gentle
                    stirring. High heat makes them rubbery and weeping. Remove from heat
                    while they still look slightly underdone — they finish in the pan.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Fresh eggs poach better.</span>
                        The white of a fresh egg is tighter and holds together better in the water.
                    Older eggs spread. A small splash of vinegar in the water helps the white
                    set quickly around the yolk.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Don't boil eggs at a rolling boil.</span>
                        A vigorous boil makes the whites tough and rubbery. Bring to a boil, then
                    reduce to a gentle simmer for the entire cooking time.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Cream and milk scorch easily — never walk away.</span>
                        When heating dairy on the stove, use medium-low heat and stir regularly.
                    Scorched milk at the bottom of a pan ruins the flavour of the entire batch.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Temper eggs before adding to hot liquid.</span>
                        When making custards or sauces, slowly whisk a little of the hot liquid
                    into the eggs first before combining everything. Adding eggs straight into
                    hot liquid scrambles them instantly.
                    </div>
                </li>
            </ol>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Egg Cooking Times</h2>
            <div class="table-wrapper">
                <table class="guide-table">
                    <thead>
                        <tr>
                            <th>Method</th>
                            <th>Time</th>
                            <th>Result</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Soft boiled</td>
                            <td>6 min</td>
                            <td>Fully set white, runny yolk.</td>
                        </tr>
                        <tr>
                            <td>Jammy / medium boiled</td>
                            <td>7 – 8 min</td>
                            <td>Fully set white, yolk soft and jammy in the centre.</td>
                        </tr>
                        <tr>
                            <td>Hard boiled</td>
                            <td>10 – 11 min</td>
                            <td>Fully set white and yolk. No grey ring if not overcooked.</td>
                        </tr>
                        <tr>
                            <td>Poached</td>
                            <td>3 – 4 min</td>
                            <td>Set white, runny yolk. Simmer at 80–85°C.</td>
                        </tr>
                        <tr>
                            <td>Fried (over easy)</td>
                            <td>2 – 3 min + flip</td>
                            <td>Set white, barely touched yolk. Medium heat.</td>
                        </tr>
                        <tr>
                            <td>Scrambled (low heat)</td>
                            <td>4 – 6 min</td>
                            <td>Soft, creamy curds. Remove just before fully set.</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p class="section-note">
                Starting from eggs placed into already-boiling water. Times produce consistent
results for large eggs at room temperature.
            </p>
        </section>

    </main>
</asp:Content>
