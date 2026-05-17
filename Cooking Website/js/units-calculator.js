/* ── 1. CONVERSION DATA ─────────────────────────────────────── */


const VOLUME_UNITS = [
    { key: "ml", label: "Millilitre", abbr: "ml", factor: 1 },
    { key: "tsp", label: "Teaspoon", abbr: "tsp", factor: 4.92892 },
    { key: "tbsp", label: "Tablespoon", abbr: "tbsp", factor: 14.7868 },
    { key: "floz", label: "Fluid Ounce", abbr: "fl oz", factor: 29.5735 },
    { key: "cup", label: "Cup", abbr: "cup", factor: 236.588 },
    { key: "pint", label: "Pint (US)", abbr: "pt", factor: 473.176 },
    { key: "quart", label: "Quart (US)", abbr: "qt", factor: 946.353 },
    { key: "gallon", label: "Gallon (US)", abbr: "gal", factor: 3785.41 },
    { key: "liter", label: "Litre", abbr: "l", factor: 1000 },
];

const MASS_UNITS = [
    { key: "mg", label: "Milligram", abbr: "mg", factor: 0.001 },
    { key: "g", label: "Gram", abbr: "g", factor: 1 },
    { key: "kg", label: "Kilogram", abbr: "kg", factor: 1000 },
    { key: "oz", label: "Ounce", abbr: "oz", factor: 28.3495 },
    { key: "lb", label: "Pound", abbr: "lb", factor: 453.592 },
];


/* ── 2. CONVERSION LOGIC ─────────────────────────────────────── */

function convert(amount, fromKey, unitList) {
    const fromUnit = unitList.find(u => u.key === fromKey);
    if (!fromUnit || isNaN(amount) || amount === "") return null;

    // Step 1: convert input → base unit
    const inBaseUnits = amount * fromUnit.factor;

    // Step 2: convert base unit → every other unit and return a map
    const results = {};
    unitList.forEach(function (unit) {
        results[unit.key] = inBaseUnits / unit.factor;
    });

    return results;
}


/* ── 3. NUMBER FORMATTING ────────────────────────────────────── */

function formatNumber(value) {
    if (value === 0) return "0";

    const absValue = Math.abs(value);

    // Use exponential notation for extremes (very large or very tiny)
    if (absValue < 0.0001 || absValue >= 1_000_000) {
        return value.toPrecision(4).replace(/\.?0+$/, "");
    }

    // For normal range, up to 4 decimal places, no trailing zeros
    return parseFloat(value.toFixed(4)).toString();
}


/* ── 4. RENDERING RESULT CARDS ───────────────────────────────── */

function renderResults(resultsMap, fromKey, unitList, gridElement) {
    // Clear whatever was there before
    gridElement.innerHTML = "";

    // No value entered yet → show a friendly prompt
    if (resultsMap === null) {
        gridElement.innerHTML = '<p class="results-placeholder">Enter an amount above to see conversions.</p>';
        return;
    }

    // Build one card per unit
    unitList.forEach(function (unit) {
        const convertedValue = resultsMap[unit.key];
        const isSource = (unit.key === fromKey);   // highlight the "from" unit

        const card = document.createElement("div");
        card.className = "result-card" + (isSource ? " is-source" : "");

        card.innerHTML =
            '<div class="result-card-unit">' + unit.label + '</div>' +
            '<div class="result-card-value">' + formatNumber(convertedValue) + '</div>' +
            '<div class="result-card-abbr">' + unit.abbr + '</div>';

        gridElement.appendChild(card);
    });
}


/* ── 5. RUNNING A CONVERSION ─────────────────────────────────── */

function runConversion(amountInputId, unitSelectId, unitList, resultsGridId) {
    const amount = parseFloat(document.getElementById(amountInputId).value);
    const fromKey = document.getElementById(unitSelectId).value;
    const grid = document.getElementById(resultsGridId);

    // convert() returns null when the amount field is empty or invalid
    const results = convert(amount, fromKey, unitList);

    renderResults(results, fromKey, unitList, grid);
}


/* ── 6. TAB SWITCHING ────────────────────────────────────────── */

function setupTabs() {
    const tabButtons = document.querySelectorAll(".tab-btn");
    const panels = document.querySelectorAll(".calc-panel");

    tabButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            const targetTab = button.getAttribute("data-tab");

            // Update button states
            tabButtons.forEach(function (btn) {
                btn.classList.remove("active");
                btn.setAttribute("aria-selected", "false");
            });
            button.classList.add("active");
            button.setAttribute("aria-selected", "true");

            // Show the matching panel, hide others
            panels.forEach(function (panel) {
                if (panel.id === "panel-" + targetTab) {
                    panel.classList.add("active");
                } else {
                    panel.classList.remove("active");
                }
            });
        });
    });
}


/* ── 7. ATTACHING EVENTS TO INPUTS ───────────────────────────── */

function setupVolumeEvents() {
    const amountInput = document.getElementById("vol-amount");
    const unitSelect = document.getElementById("vol-unit");

    function update() {
        runConversion("vol-amount", "vol-unit", VOLUME_UNITS, "vol-results");
    }

    amountInput.addEventListener("input", update);
    amountInput.addEventListener("change", update);
    unitSelect.addEventListener("change", update);
}

function setupMassEvents() {
    const amountInput = document.getElementById("mass-amount");
    const unitSelect = document.getElementById("mass-unit");

    function update() {
        runConversion("mass-amount", "mass-unit", MASS_UNITS, "mass-results");
    }

    amountInput.addEventListener("input", update);
    amountInput.addEventListener("change", update);
    unitSelect.addEventListener("change", update);
}


/* ── 8. INITIALISATION ───────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {
    setupTabs();
    setupVolumeEvents();
    setupMassEvents();

    // Show placeholder text in both grids on first load
    renderResults(null, null, VOLUME_UNITS, document.getElementById("vol-results"));
    renderResults(null, null, MASS_UNITS, document.getElementById("mass-results"));
});