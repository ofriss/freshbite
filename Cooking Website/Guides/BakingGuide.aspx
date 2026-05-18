<%@ Page Language="C#" MasterPageFile="~/Guides/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="BakingGuide.aspx.cs" Inherits="Cooking_Website.BakingGuide" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    Baking Guide
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Bread &amp; Baking</h1>
            <p class="guide-intro">
                Baking is the most precise discipline in the kitchen. Unlike cooking, where
            intuition and adjustment are part of the process, baking depends on the right
            ratios, temperatures, and techniques from the start. Understanding what each
            ingredient does gives you the confidence to troubleshoot and adapt.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Preparation</h2>
            <p class="guide-prep">
                Always weigh ingredients rather than measuring by volume — a cup of flour
            can vary by up to 30% depending on how it's scooped. Bring butter, eggs,
            and dairy to room temperature unless the recipe specifically calls for cold
            (as in pastry, where cold fat creates flakiness). Preheat the oven fully
            before baking — a partially heated oven affects rise and crust development.
            For bread, proofing dough in a warm, draught-free spot (around 24–27°C) gives
            the yeast the best conditions to work. Score bread just before it goes into
            the oven so the cuts are clean and steam can escape in a controlled way.
            </p>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Weigh everything — always.</span>
                        Volume measurements are inconsistent, especially for flour. A kitchen scale
                    is the single most impactful tool for consistent baking results.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Don't overmix batter or dough.</span>
                        Overmixing develops gluten in batters and makes cakes and muffins tough.
                    Mix just until the dry ingredients are incorporated — a few streaks are fine.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Steam is essential for crusty bread.</span>
                        Baking bread in a covered Dutch oven or adding a tray of boiling water to
                    the oven traps steam, which keeps the crust soft in the first phase and
                    allows maximum oven spring. Remove the lid or water in the second phase
                    to develop colour and crunch.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">The windowpane test tells you when bread dough is ready.</span>
                        Stretch a small piece of dough between your fingers. If it stretches thin
                    enough to see light through without tearing, gluten is fully developed
                    and the dough is ready to proof.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Test cakes with a skewer, not just time.</span>
                        Oven temperatures vary. A skewer inserted into the centre should come out
                    clean or with a few moist crumbs — not wet batter. Time is a guide,
                    not a guarantee.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Cool baked goods properly before cutting.</span>
                        Bread continues to cook internally as it cools. Cutting too early produces
                    a gummy crumb. Wait at least 30–45 minutes for loaves; 10–15 for rolls.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Fat coats flour and tenderises.</span>
                        In cakes, fat shortens gluten strands, producing a tender crumb.
                    In pastry, keeping fat cold and in distinct pieces creates steam pockets
                    that produce flakiness. The temperature of your fat determines the texture.
                    </div>
                </li>
            </ol>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Oven Temperature Guide</h2>
            <div class="table-wrapper">
                <table class="guide-table">
                    <thead>
                        <tr>
                            <th>Baked Good</th>
                            <th>Temperature (°C)</th>
                            <th>Temperature (°F)</th>
                            <th>Notes</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Artisan bread (covered)</td>
                            <td>230 – 250°C</td>
                            <td>445 – 480°F</td>
                            <td>Start covered for 20 min, then uncover to colour.</td>
                        </tr>
                        <tr>
                            <td>Baguette / rolls</td>
                            <td>220°C</td>
                            <td>425°F</td>
                            <td>With steam in the first 10 minutes.</td>
                        </tr>
                        <tr>
                            <td>Sandwich loaf</td>
                            <td>190 – 200°C</td>
                            <td>375 – 390°F</td>
                            <td>Internal temp of 93–96°C when done.</td>
                        </tr>
                        <tr>
                            <td>Cakes (layer / sponge)</td>
                            <td>170 – 180°C</td>
                            <td>340 – 355°F</td>
                            <td>Lower temp prevents doming and cracking.</td>
                        </tr>
                        <tr>
                            <td>Muffins / cupcakes</td>
                            <td>190°C</td>
                            <td>375°F</td>
                            <td>Higher heat gives a better rise and dome.</td>
                        </tr>
                        <tr>
                            <td>Shortcrust pastry</td>
                            <td>180 – 190°C</td>
                            <td>355 – 375°F</td>
                            <td>Blind bake at 190°C, finish filled at 180°C.</td>
                        </tr>
                        <tr>
                            <td>Cookies</td>
                            <td>170 – 190°C</td>
                            <td>340 – 375°F</td>
                            <td>Lower temp = chewy; higher temp = crispy edges.</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p class="section-note">
                These are conventional oven temperatures. Fan-assisted (convection) ovens run
roughly 15–20°C hotter — reduce accordingly, or consult your recipe.
            </p>
        </section>

    </main>
</asp:Content>
