<%@ Page Language="C#" MasterPageFile="~/Guides/GuideSidebar.Master" AutoEventWireup="true" CodeBehind="SoupsGuide.aspx.cs" Inherits="Cooking_Website.SoupsGuide" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Title" runat="server">
    Soups Guide
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <main class="guide-container">

        <div class="guide-header">
            <span class="guide-tag">Cooking Guide</span>
            <h1>Soups &amp; Stews</h1>
            <p class="guide-intro">
                Soups and stews are among the most forgiving and rewarding things to cook.
            They improve with time, reward patience, and are built on a foundation of
            good stock and layered seasoning. The difference between a flat, watery soup
            and a deeply flavoured one usually comes down to how well you build the base.
            </p>
        </div>

        <section class="guide-section">
            <h2 class="section-title">Tips</h2>
            <ol class="tips-list">
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Start with a proper base — the soffritto.</span>
                        Onion, celery, and carrot (or leek and garlic) cooked slowly in fat until
                    soft and sweet is the foundation of almost every great soup and stew.
                    Don't rush this step — it takes 10–15 minutes and builds the backbone of flavour.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Use good stock — or make your own.</span>
                        Stock is the single biggest determinant of a soup's flavour. Store-bought
                    stock varies enormously in quality. Homemade stock, even a simple one,
                    produces a noticeably better result. Reduce it before using for more depth.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Season in layers, not all at once.</span>
                        Add salt at each stage — with the aromatics, when you add the liquid,
                    and again at the end. This builds complexity rather than just saltiness.
                    Always taste and adjust at the end.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Simmer, never boil.</span>
                        A rolling boil makes proteins in stock and meat tough and cloudy.
                    A gentle simmer — with just a few bubbles breaking the surface — keeps
                    everything tender and the broth clear.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Brown meat before adding to stews.</span>
                        Searing meat in batches until deeply browned adds layers of flavour
                    through the Maillard reaction. Don't skip this step — it's what separates
                    a great stew from a boiled one.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Deglaze after browning.</span>
                        After browning meat or vegetables, add a splash of wine, stock, or water
                    and scrape up all the browned bits from the bottom of the pot.
                    Those bits are pure flavour.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Soups and stews are almost always better the next day.</span>
                        Overnight in the fridge allows flavours to meld and deepen. Make a big
                    batch and plan for leftovers — they will be better the second time.
                    </div>
                </li>
                <li>
                    <div class="tip-body">
                        <span class="tip-title">Finish with something bright.</span>
                        A squeeze of lemon, a splash of vinegar, or fresh herbs added at the end
                    lifts a long-cooked soup out of heaviness and brings it to life.
                    </div>
                </li>
            </ol>
        </section>

    </main>
</asp:Content>
