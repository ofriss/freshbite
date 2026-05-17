<%@ Page Language="C#" MasterPageFile="~/Guide/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="PastaGuide.aspx.cs" Inherits="Cooking_Website.PastaGuide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Pasta &amp; Grains</h1>
            <p class="guide-intro">
                Pasta and grains are the backbone of countless cuisines. They're simple to cook
            but easy to get wrong — from gluey rice to blown-out pasta. The fundamentals
            are water ratio, heat control, and timing. Get those right and the rest follows.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Preparation</h2>
            <p class="guide-prep">
                Salt your pasta water until it tastes like the sea — this is the only chance to
            season the pasta itself. Use a large pot with plenty of water so the pasta can
            move freely and cook evenly. For grains like rice, rinsing removes excess surface
            starch which can cause clumping; skip rinsing for risotto and rice pudding where
            starch is part of the texture. Toasting dry grains like rice, farro, or barley
            in a dry pan or with butter before adding liquid deepens the flavour considerably.
            </p>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Always salt the water — generously.</span>
                        Under-salted water produces flat, bland pasta no matter how good the sauce.
                    Use at least 1 tablespoon of salt per litre of water.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Cook pasta al dente — it will cook further in the sauce.</span>
                        Pull pasta 1–2 minutes before the package says it's done, then finish it
                    directly in the sauce with a splash of pasta water. It absorbs the sauce
                    and becomes part of the dish rather than just sitting in it.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Save the pasta water — it's liquid gold.</span>
                        Starchy pasta water emulsifies sauces and helps them cling to pasta.
                    Always reserve at least a cup before draining. Add it to the sauce
                    a splash at a time to adjust consistency.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Never rinse cooked pasta.</span>
                        Rinsing removes the surface starch that helps sauce adhere to pasta.
                    Only rinse pasta if using it in a cold salad.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Match pasta shape to sauce weight.</span>
                        Thin, delicate pasta (angel hair, linguine) suits light, oil-based sauces.
                    Chunky, ridged pasta (rigatoni, penne) holds thick, meat-based sauces.
                    Flat sheets (pappardelle) work with rich ragù.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Use the absorption method for fluffy rice.</span>
                        Rinse, then use a 1:1.5 ratio of rice to water, bring to a boil, cover,
                    reduce to the lowest possible heat for 12 minutes, then rest covered for
                    10 minutes. Don't lift the lid during cooking.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Stir risotto consistently but not constantly.</span>
                        Add warm stock ladle by ladle, stirring enough to keep the rice moving
                    and releasing starch. Over-stirring makes it gluey; under-stirring causes
                    uneven cooking. Finish off heat with cold butter and parmesan (mantecatura).
                    </div>
                </li>
            </ol>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Grain Cooking Reference</h2>
            <div class="table-wrapper">
                <table class="guide-table">
                    <thead>
                        <tr>
                            <th>Grain</th>
                            <th>Water Ratio</th>
                            <th>Cook Time</th>
                            <th>Notes</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>White rice (long grain)</td>
                            <td>1 : 1.5</td>
                            <td>12 min + 10 rest</td>
                            <td>Do not lift lid during cooking.</td>
                        </tr>
                        <tr>
                            <td>Brown rice</td>
                            <td>1 : 2</td>
                            <td>40 – 45 min</td>
                            <td>Nuttier flavour, chewier texture.</td>
                        </tr>
                        <tr>
                            <td>Quinoa</td>
                            <td>1 : 1.75</td>
                            <td>15 min + 5 rest</td>
                            <td>Rinse well to remove bitter saponins.</td>
                        </tr>
                        <tr>
                            <td>Couscous</td>
                            <td>1 : 1.25</td>
                            <td>5 min (just steep)</td>
                            <td>Pour boiling water over, cover, fluff after 5 min.</td>
                        </tr>
                        <tr>
                            <td>Farro (pearled)</td>
                            <td>1 : 2.5</td>
                            <td>25 – 30 min</td>
                            <td>Chewy and nutty. Toast before cooking for more depth.</td>
                        </tr>
                        <tr>
                            <td>Polenta (coarse)</td>
                            <td>1 : 4</td>
                            <td>30 – 40 min</td>
                            <td>Stir frequently. Finish with butter and parmesan.</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p class="section-note">
                Water ratios are for the absorption method (1 cup of dry grain). Actual times
    may vary slightly by brand and age of the grain.
            </p>
        </section>

    </main>
</asp:Content>
