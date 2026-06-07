// units-calculator.js — converts a single volume or mass amount into every other unit, shown as result cards

/* ── 1. CONVERSION DATA ─────────────────────────────────────── */
// Each unit stores how many base units it equals (ml for volume, g for mass).

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


/* ── 2. NUMBER FORMATTING ────────────────────────────────────── */

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


/* ── 3. RENDER A CONVERSION ──────────────────────────────────── */
// Reads the panel's amount + chosen unit and rebuilds its result cards.
// One step: convert input -> base units (amount * fromUnit.factor), then
// base units -> each unit (/ unit.factor). No intermediate map needed.

function renderConversions(amountId, unitId, units, gridId) {
    const amount = parseFloat(document.getElementById(amountId).value);
    const fromKey = document.getElementById(unitId).value;
    const grid = document.getElementById(gridId);
    const fromUnit = units.find(u => u.key === fromKey);

    grid.innerHTML = "";

    // No (or invalid) amount entered yet → show a friendly prompt
    if (!fromUnit || isNaN(amount)) {
        grid.innerHTML = '<p class="results-placeholder">Enter an amount above to see conversions.</p>';
        return;
    }

    const inBaseUnits = amount * fromUnit.factor;

    // Build one card per unit, highlighting the "from" unit
    units.forEach(function (unit) {
        const value = inBaseUnits / unit.factor;
        const isSource = (unit.key === fromKey);

        const card = document.createElement("div");
        card.className = "result-card" + (isSource ? " is-source" : "");
        card.innerHTML =
            '<div class="result-card-unit">' + unit.label + '</div>' +
            '<div class="result-card-value">' + formatNumber(value) + '</div>' +
            '<div class="result-card-abbr">' + unit.abbr + '</div>';

        grid.appendChild(card);
    });
}


/* ── 4. TAB SWITCHING ────────────────────────────────────────── */

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


/* ── 5. WIRE UP A PANEL ──────────────────────────────────────── */

// Re-render whenever the amount or unit changes, and once now for the initial state
function setupCalculator(amountId, unitId, units, gridId) {
    function update() {
        renderConversions(amountId, unitId, units, gridId);
    }

    document.getElementById(amountId).addEventListener("input", update);
    document.getElementById(unitId).addEventListener("change", update);
    update();
}


/* ── 6. INITIALISATION ───────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {
    setupTabs();
    setupCalculator("vol-amount", "vol-unit", VOLUME_UNITS, "vol-results");
    setupCalculator("mass-amount", "mass-unit", MASS_UNITS, "mass-results");
});
