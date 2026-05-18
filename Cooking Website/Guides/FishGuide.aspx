<%@ Page Language="C#" MasterPageFile="~/Guides/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="FishGuide.aspx.cs" Inherits="Cooking_Website.FishGuide" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    Fish Guide
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Fish &amp; Seafood</h1>
            <p class="guide-intro">
                Fish and seafood cook faster than almost anything else in the kitchen — and they
            go from perfect to overdone in seconds. The key principles are freshness, dryness,
            and restraint with heat. Most fish needs far less cooking than people think.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Preparation</h2>
            <p class="guide-prep">
                Always check fillets for pin bones by running your finger along the flesh against
            the grain — remove them with tweezers or needle-nose pliers. Pat fish completely dry
            before cooking; surface moisture is the enemy of a good sear. For whole fish, score
            the skin diagonally two or three times on each side so heat penetrates evenly and the
            skin doesn't curl. For shellfish like mussels and clams, scrub the shells under cold
            water and remove any beards just before cooking — not ahead of time, as it shortens
            their life. Shrimp should be deveined for texture and presentation.
            </p>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Buy the freshest fish you can find.</span>
                        Fresh fish smells like the sea, not like "fish." Eyes should be clear and
                    bright, flesh should be firm and spring back when pressed, and gills should
                    be red. If it smells strongly, don't buy it.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Most fish is done earlier than you think.</span>
                        Fish is fully cooked when it just begins to flake and turns opaque.
                    A common rule: 10 minutes per 2.5cm of thickness at 200°C. Pull it just
                    before it looks done — residual heat finishes the job.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Don't move fish in the pan until it releases naturally.</span>
                        Fish skin sticks to a hot pan until the proteins set and release on their own.
                    If it's resisting, it's not ready to flip. Wait — it will let go.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Cook skin-side down for most of the time.</span>
                        For pan-fried fish, cook 70–80% of the time on the skin side over medium-high
                    heat, then flip briefly. This crisps the skin and gently finishes the flesh.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Acid brightens everything.</span>
                        A squeeze of lemon or a splash of white wine right at the end cuts through
                    richness and lifts the natural flavour. Add it off the heat.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Shellfish open when they're done — discard any that don't.</span>
                        Mussels and clams that remain closed after cooking should be thrown away.
                    Those that open are safe to eat.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Poaching is the most forgiving method.</span>
                        Gentle poaching in flavoured liquid (court bouillon, olive oil, coconut milk)
                    makes it almost impossible to overcook delicate fish like sole or cod.
                    </div>
                </li>
            </ol>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Doneness Guide</h2>
            <div class="table-wrapper">
                <table class="guide-table">
                    <thead>
                        <tr>
                            <th>Fish Type</th>
                            <th>Target Temp (°C)</th>
                            <th>Target Temp (°F)</th>
                            <th>Texture at Doneness</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Salmon (medium)</td>
                            <td>50 – 52°C</td>
                            <td>122 – 126°F</td>
                            <td>Translucent, silky centre. Just beginning to flake.</td>
                        </tr>
                        <tr>
                            <td>Salmon (well done)</td>
                            <td>60°C</td>
                            <td>140°F</td>
                            <td>Fully opaque, flakes easily. Drier texture.</td>
                        </tr>
                        <tr>
                            <td>Tuna (medium rare)</td>
                            <td>43 – 46°C</td>
                            <td>110 – 115°F</td>
                            <td>Warm red centre. Firm but yielding.</td>
                        </tr>
                        <tr>
                            <td>White fish (cod, halibut)</td>
                            <td>58 – 60°C</td>
                            <td>136 – 140°F</td>
                            <td>Opaque throughout, flakes in large moist pieces.</td>
                        </tr>
                        <tr>
                            <td>Shrimp / Prawns</td>
                            <td>60°C</td>
                            <td>140°F</td>
                            <td>Pink and opaque. Curled into a loose C shape — not a tight O.</td>
                        </tr>
                        <tr>
                            <td>Scallops</td>
                            <td>52 – 55°C</td>
                            <td>125 – 130°F</td>
                            <td>Opaque on the outside, slightly translucent in the centre.</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p class="section-note">
                Fish doneness is less about a single temperature and more about texture and opacity.
These are general guidelines — oily fish like salmon and tuna are often preferred
slightly under the well-done threshold.
            </p>
        </section>

    </main>
</asp:Content>
