<%@ Page Language="C#" MasterPageFile="~/Guides/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="VegetablesGuide.aspx.cs" Inherits="Cooking_Website.VegetablesGuide" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    Vegetables Guide
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Vegetables</h1>
            <p class="guide-intro">
                Vegetables are endlessly varied — roots, leaves, stems, pods, and fungi all
            behave differently under heat. The common thread is understanding how heat
            changes texture and flavour, and choosing the right method to bring out the
            best in each vegetable rather than just cooking it until it's soft.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Preparation</h2>
            <p class="guide-prep">
                Wash all vegetables thoroughly, but dry them well before roasting or sautéing —
            water causes steaming instead of browning. Cut vegetables to a uniform size so
            they cook evenly; uneven pieces mean some are undercooked while others are mushy.
            For bitter greens like kale or Brussels sprouts, a quick blanch before finishing
            in a hot pan removes harshness. Salting cut eggplant or zucchini and letting it
            sit for 20–30 minutes draws out excess moisture and concentrates flavour.
            For dense root vegetables like carrots or beets, par-cooking before roasting
            saves time and ensures they cook through without the exterior burning.
            </p>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">High heat roasting beats low and slow for most vegetables.</span>
                        200–220°C gives you caramelisation and browning. Low temperatures just steam
                    vegetables in their own moisture and produce a limp, flavourless result.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Don't overcrowd the roasting pan.</span>
                        Vegetables need space around them for moisture to escape. A crowded pan traps
                    steam and you get stewed vegetables instead of roasted ones.
                    Use two trays if needed.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Season generously with oil and salt before roasting.</span>
                        Every surface should be lightly coated with oil. Salt draws out moisture
                    initially which then evaporates, concentrating flavour and aiding browning.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Blanch and shock for vibrant colour.</span>
                        Boiling green vegetables briefly then plunging into ice water sets the bright
                    colour and stops cooking instantly. Essential for green beans, broccoli, and
                    asparagus if serving at room temperature or in salads.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Add aromatics at the right time.</span>
                        Garlic burns quickly — add it mid-way through sautéing, not at the start.
                    Hardy herbs like rosemary and thyme can go in early; delicate ones like
                    basil and parsley should be added off the heat.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Acid balances and lifts vegetable dishes.</span>
                        A splash of vinegar or lemon juice at the end brightens the flavour of
                    almost any vegetable dish, especially roasted or braised ones.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Roast cut-side down for maximum caramelisation.</span>
                        For halved Brussels sprouts, tomatoes, or fennel, place the flat cut side
                    directly on the tray for direct contact with the hot surface.
                    </div>
                </li>
            </ol>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Blanching Times</h2>
            <div class="table-wrapper">
                <table class="guide-table">
                    <thead>
                        <tr>
                            <th>Vegetable</th>
                            <th>Blanching Time</th>
                            <th>Notes</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Asparagus (thin)</td>
                            <td>1 – 2 min</td>
                            <td>Tender-crisp. Shock immediately.</td>
                        </tr>
                        <tr>
                            <td>Broccoli florets</td>
                            <td>2 – 3 min</td>
                            <td>Bright green, just tender. Don't overcook.</td>
                        </tr>
                        <tr>
                            <td>Green beans</td>
                            <td>3 – 4 min</td>
                            <td>Crisp and vibrant. Shock well.</td>
                        </tr>
                        <tr>
                            <td>Peas (fresh)</td>
                            <td>1 – 2 min</td>
                            <td>Sweet and bright. Easy to overcook.</td>
                        </tr>
                        <tr>
                            <td>Spinach / leafy greens</td>
                            <td>30 – 60 sec</td>
                            <td>Just wilted. Squeeze out water after shocking.</td>
                        </tr>
                        <tr>
                            <td>Carrot (sliced)</td>
                            <td>3 – 4 min</td>
                            <td>Tender but still with bite.</td>
                        </tr>
                        <tr>
                            <td>Brussels sprouts (whole)</td>
                            <td>4 – 5 min</td>
                            <td>Par-cook before roasting for better results.</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p class="section-note">
                Times are for boiling salted water. Transfer immediately to ice water after blanching
to stop cooking and preserve colour.
            </p>
        </section>

    </main>
</asp:Content>
