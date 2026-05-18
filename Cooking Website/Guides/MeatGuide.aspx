<%@ Page Language="C#" MasterPageFile="~/Guides/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="MeatGuide.aspx.cs" Inherits="Cooking_Website.MeatGuide" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    Meat Guide
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <main class="guide-container">

        <!-- ── Header ── -->
        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Meat</h1>
            <p class="guide-intro">
                Meat is one of the most technique-sensitive ingredients in the kitchen.
            Heat transforms its proteins, collagen, and fat in very different ways depending
            on the cut, cooking method, and target temperature. Understanding these fundamentals
            will help you cook every cut — from a delicate tenderloin to a tough brisket —
            exactly the way it should be cooked.
            </p>
        </div>

        <!-- ── Preparation ── -->
        <section class="guide-section">
            <h2 class="section-title">Preparation</h2>
            <p class="guide-prep">
                Most cuts benefit from being trimmed of excess hard fat — leave a thin layer for
            flavour and basting, but thick patches won't render in time and can cause flare-ups.
            For tougher cuts like flank, skirt, or chuck, a marinade with an acidic component
            (citrus juice, vinegar, yogurt) helps break down surface fibres before cooking.
            For premium cuts like ribeye or tenderloin, keep preparation minimal — a dry brine
            of salt and pepper applied the night before is all they need. Always tie roasts with
            butcher's twine to keep an even shape for uniform cooking.
            </p>
        </section>

        <!-- ── Tips ── -->
        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Always bring meat to room temperature before cooking.</span>
                        Cold meat straight from the fridge cooks unevenly — the outside overcooks
                    before the centre reaches the right temperature. Take it out 30–45 minutes ahead.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Pat the surface completely dry.</span>
                        Moisture on the surface creates steam, which prevents a proper sear.
                    Use paper towels and press firmly on all sides before seasoning.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Season generously with salt — and early.</span>
                        Salt draws moisture out initially, then the meat reabsorbs it along with the seasoning.
                    Season at least 45 minutes before cooking, or right before hitting the pan.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Use a meat thermometer — never guess.</span>
                        Colour, firmness, and "the poke test" are all unreliable. An instant-read thermometer
                    is the only accurate way to hit your target doneness every time.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Sear over high heat, finish over low heat for thick cuts.</span>
                        For steaks thicker than 2.5cm, a reverse sear (low oven first, then hot pan)
                    gives you edge-to-edge even cooking with a better crust.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Always rest the meat after cooking.</span>
                        Resting allows the juices to redistribute. Without it, they run straight out
                    when you cut. Rest for at least 5 minutes for steaks, 15–20 for roasts.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Cut against the grain.</span>
                        Slicing perpendicular to the muscle fibres shortens them, making every bite
                    noticeably more tender. This matters most for tougher cuts like flank or skirt.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Match the cooking method to the cut.</span>
                        Tender cuts (tenderloin, ribeye, sirloin) suit fast, dry heat — grilling or pan-searing.
                    Tough cuts (brisket, chuck, shank) need long, slow, moist heat to break down collagen
                    into gelatin.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Don't crowd the pan.</span>
                        Too many pieces lower the pan temperature, causing the meat to steam rather than sear.
                    Cook in batches if needed and make sure there is space around each piece.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Let the fond work for you.</span>
                        The brown bits stuck to the pan after searing are packed with flavour.
                    Deglaze with stock, wine, or water to build a quick pan sauce.
                    </div>
                </li>
            </ol>
        </section>

        <!-- ── Cooking Table ── -->
        <section class="guide-section">
            <h2 class="section-title">Internal Temperatures</h2>
            <p class="section-note">
                Measured at the thickest part of the cut, away from bone. Remove meat from heat
            3–5°C before target — it will rise further while resting.
            </p>

            <div class="table-wrapper">
                <table class="guide-table">
                    <thead>
                        <tr>
                            <th>Doneness</th>
                            <th>Internal Temp (°C)</th>
                            <th>Internal Temp (°F)</th>
                            <th>Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td><span class="doneness-badge rare">Rare</span></td>
                            <td>49 – 54°C</td>
                            <td>120 – 130°F</td>
                            <td>Cool, bright red centre. Very soft.</td>
                        </tr>
                        <tr>
                            <td><span class="doneness-badge medium-rare">Medium Rare</span></td>
                            <td>55 – 59°C</td>
                            <td>131 – 139°F</td>
                            <td>Warm red centre. Most tender, maximum juiciness.</td>
                        </tr>
                        <tr>
                            <td><span class="doneness-badge medium">Medium</span></td>
                            <td>60 – 65°C</td>
                            <td>140 – 149°F</td>
                            <td>Pink centre, firmer texture. Still juicy.</td>
                        </tr>
                        <tr>
                            <td><span class="doneness-badge medium-well">Medium Well</span></td>
                            <td>66 – 71°C</td>
                            <td>150 – 160°F</td>
                            <td>Slight pink, noticeably firmer. Less juicy.</td>
                        </tr>
                        <tr>
                            <td><span class="doneness-badge well-done">Well Done</span></td>
                            <td>72°C +</td>
                            <td>162°F +</td>
                            <td>No pink. Firm throughout. Very little moisture.</td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <p class="section-note">
                Ground meat and pork should always reach a minimum of 71°C (160°F) for food safety.
            Poultry is a separate guide — chicken and turkey require 74°C (165°F) minimum.
            </p>
        </section>

    </main>
</asp:Content>
