<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UnitsCalculator.aspx.cs" Inherits="Cooking_Website.UnitsCalculator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/css/units-calculator.css" />
    <script src="/js/units-calculator.js" defer></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <main class="main-container">

        <div class="page-title-block">
            <%-- h1 styling (font, size, weight) comes from styles.css --%>
            <h1>Units Calculator</h1>
            <%-- p color comes from styles.css (var(--secondary)) --%>
            <p>Convert any measurement found in a recipe — volume or mass.</p>
        </div>

        <!-- Category Tabs -->
        <div class="tab-bar" role="tablist" aria-label="Measurement type">
            <button class="tab-btn active" role="tab" aria-selected="true" data-tab="volume" type="button">Volume</button>
            <button class="tab-btn" role="tab" aria-selected="false" data-tab="mass" type="button">Mass &amp; Weight</button>
        </div>

        <!-- ═══════════════════ VOLUME PANEL ═══════════════════ -->
        <section class="calc-panel active" id="panel-volume" role="tabpanel">

            <div class="input-row">
                <div class="input-group">
                    <label class="input-label" for="vol-amount">Amount</label>
                    <input class="number-input" id="vol-amount" type="number" min="0" step="any"
                        placeholder="e.g. 2.5" autocomplete="off" />
                </div>
                <div class="input-group">
                    <label class="input-label" for="vol-unit">From unit</label>
                    <select class="unit-select" id="vol-unit">
                        <option value="ml">Millilitre (ml)</option>
                        <option value="tsp">Teaspoon (tsp)</option>
                        <option value="tbsp" selected>Tablespoon (tbsp)</option>
                        <option value="floz">Fluid Ounce (fl oz)</option>
                        <option value="cup">Cup</option>
                        <option value="pint">Pint (US)</option>
                        <option value="quart">Quart (US)</option>
                        <option value="gallon">Gallon (US)</option>
                        <option value="liter">Litre (l)</option>
                    </select>
                </div>
            </div>

            <div class="results-grid" id="vol-results">
                <!-- Cards injected by JS -->
            </div>

            <p class="hint-text">All volume conversions use US customary units where applicable.</p>
        </section>

        <!-- ═══════════════════ MASS PANEL ═══════════════════ -->
        <section class="calc-panel" id="panel-mass" role="tabpanel">

            <div class="input-row">
                <div class="input-group">
                    <label class="input-label" for="mass-amount">Amount</label>
                    <input class="number-input" id="mass-amount" type="number" min="0" step="any"
                        placeholder="e.g. 500" autocomplete="off" />
                </div>
                <div class="input-group">
                    <label class="input-label" for="mass-unit">From unit</label>
                    <select class="unit-select" id="mass-unit">
                        <option value="mg">Milligram (mg)</option>
                        <option value="g" selected>Gram (g)</option>
                        <option value="kg">Kilogram (kg)</option>
                        <option value="oz">Ounce (oz)</option>
                        <option value="lb">Pound (lb)</option>
                    </select>
                </div>
            </div>

            <div class="results-grid" id="mass-results">
                <!-- Cards injected by JS -->
            </div>

            <p class="hint-text">All mass conversions are exact metric ↔ imperial equivalents.</p>
        </section>

    </main>
</asp:Content>
