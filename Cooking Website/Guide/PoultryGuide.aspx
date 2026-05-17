<%@ Page Language="C#" MasterPageFile="~/Guide/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="PoultryGuide.aspx.cs" Inherits="Cooking_Website.PoultryGuide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Poultry</h1>
            <p class="guide-intro">
                Poultry is unforgiving in two directions — undercook it and it's unsafe,
            overcook it and it turns dry and stringy. The difference between a perfectly
            juicy chicken breast and a rubbery one is often just a few degrees. Learning
            to control heat and moisture is everything with poultry.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Preparation</h2>
            <p class="guide-prep">
                Always pat poultry completely dry before cooking — wet skin steams instead of crisping.
            For whole birds, air-drying uncovered in the fridge overnight dramatically improves
            the skin. Spatchcocking (removing the backbone and flattening the bird) is the most
            reliable way to roast a whole chicken evenly, as it eliminates the problem of the
            breast overcooking before the thighs are done. Brining — either wet or dry — adds
            seasoning deep into the meat and helps it retain moisture during cooking. Never
            wash raw poultry; it spreads bacteria without making the bird safer.
            </p>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Never rely on colour alone to judge doneness.</span>
                        Pink juice or slightly pink meat near the bone doesn't always mean undercooked.
                    Use a thermometer — it's the only reliable method.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Dark meat and white meat cook differently.</span>
                        Breasts dry out quickly past 70°C. Thighs and legs are better at 80–85°C
                    where the collagen has time to break down and the meat stays juicy.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Dry brine at least 4 hours ahead — overnight is better.</span>
                        Salt applied directly to the skin draws out moisture, then reabsorbs it,
                    seasoning the meat from the surface inward and drying the skin for better crispness.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Start skin-side down in a cold pan for crispy skin.</span>
                        For chicken thighs especially, placing them in a cold pan and bringing the heat up
                    gradually renders the fat slowly and gives you a much crispier result than a hot pan.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Rest poultry before carving.</span>
                        A whole chicken needs at least 10–15 minutes of rest. Cutting too early lets
                    all the juices run out onto the board instead of staying in the meat.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Stuff loosely — or don't stuff at all.</span>
                        Stuffing inside the cavity slows heat transfer to the bird's core. If you
                    do stuff, make sure both the bird and the stuffing reach 74°C. Cooking
                    stuffing separately is safer and more predictable.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Butterflied breasts cook faster and more evenly.</span>
                        A thick chicken breast is notoriously uneven. Butterfly it (slice horizontally
                    without cutting all the way through and open it flat) or pound it to an even
                    thickness for consistent results.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Use the carcass — always.</span>
                        Roasted poultry bones make exceptional stock. Simmer with onion, carrot,
                    celery, and aromatics for 3–4 hours. Freeze in portions for soups and sauces.
                    </div>
                </li>
            </ol>
        </section>

        <section class="guide-section">
            <h2 class="section-title">Safe Internal Temperatures</h2>
            <div class="table-wrapper">
                <table class="guide-table">
                    <thead>
                        <tr>
                            <th>Cut</th>
                            <th>Min Temp (°C)</th>
                            <th>Min Temp (°F)</th>
                            <th>Notes</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Chicken breast</td>
                            <td>74°C</td>
                            <td>165°F</td>
                            <td>Pull at 70°C — carryover brings it up. Do not exceed 75°C.</td>
                        </tr>
                        <tr>
                            <td>Chicken thigh / leg</td>
                            <td>74°C minimum</td>
                            <td>165°F minimum</td>
                            <td>Better at 80–85°C. Collagen breaks down, meat stays juicy.</td>
                        </tr>
                        <tr>
                            <td>Whole chicken</td>
                            <td>74°C</td>
                            <td>165°F</td>
                            <td>Measure at the thickest part of the thigh, away from bone.</td>
                        </tr>
                        <tr>
                            <td>Turkey (whole)</td>
                            <td>74°C</td>
                            <td>165°F</td>
                            <td>Check both the breast and the thigh — they may finish at different times.</td>
                        </tr>
                        <tr>
                            <td>Duck breast</td>
                            <td>57°C</td>
                            <td>135°F</td>
                            <td>Duck breast is commonly served medium-rare. Score the skin deeply first.</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <p class="section-note">
                Unlike beef, poultry has no "rare" — all poultry must reach a safe minimum
    temperature throughout. These are minimums; dark meat is often better slightly higher.
            </p>
        </section>

    </main>
</asp:Content>
