// recipe.js — servings scaler, ingredient checkboxes, and a filter-preserving
// Back button for the individual recipe page.

/* ── References ───────────────────────────────────────────────── */

const backBtn = document.getElementById("back-btn");
const scalerMinus = document.getElementById("scaler-minus");
const scalerPlus = document.getElementById("scaler-plus");
const scalerValue = document.getElementById("scaler-value");
const ingredientList = document.getElementById("ingredients-list");
const clearBtn = document.getElementById("clear-checks-btn");

// Recipe id from the URL — used to key this recipe's checked ingredients
const recipeId = new URLSearchParams(window.location.search).get("id");
const STORAGE_KEY_CHECKS = "recipe_checks_" + recipeId;
const STORAGE_KEY_FILTERS = "recipes_filters";


/* ── Back button — keep the listing's filters ─────────────────── */

if (backBtn) {
    backBtn.addEventListener("click", function (e) {
        e.preventDefault();
        // Reapply saved filters if we have them, otherwise just go back to the list
        const saved = sessionStorage.getItem(STORAGE_KEY_FILTERS);
        window.location.href = saved ? "/Recipes.aspx?" + saved : "/Recipes.aspx";
    });
}


/* ── Quantity helpers ─────────────────────────────────────────── */

// Turn a quantity string ("2", "0.5", "1/2", "1 1/2") into a number, or null
// if it isn't something we can scale (e.g. "to taste").
function parseQuantity(str) {
    if (!str) return null;

    let total = 0;
    // Split on spaces so "1 1/2" becomes ["1", "1/2"], then add the pieces up
    const tokens = str.trim().split(/\s+/);

    for (const token of tokens) {
        if (/^\d+\/\d+$/.test(token)) {            // a fraction like 1/2
            const [top, bottom] = token.split("/");
            total += Number(top) / Number(bottom);
        } else if (/^\d+(\.\d+)?$/.test(token)) {  // a whole number or decimal
            total += Number(token);
        } else {
            return null;                            // not a number — leave as-is
        }
    }
    return total;
}

// Show a number cleanly as a decimal: 1.5, 2, 0.25 (rounded to 2 places).
function formatQuantity(num) {
    return parseFloat(num.toFixed(2)).toString();
}


/* ── Servings scaler ──────────────────────────────────────────── */

let currentServings = RecipeConfig.defaultServings;
const minServings = 1;
const maxServings = 100;

// Disable +/- at the min/max boundaries
function updateScalerButtons() {
    // Grey out "-" at the lowest serving count and "+" at the highest
    scalerMinus.disabled = currentServings <= minServings;
    scalerPlus.disabled = currentServings >= maxServings;
}

// Rescale every ingredient from its original amount to the current servings
function scaleIngredients() {
    // How much bigger/smaller than the recipe's default serving count we are
    const ratio = currentServings / RecipeConfig.defaultServings;

    document.querySelectorAll(".ingredient-quantity").forEach(function (el) {
        const parsed = parseQuantity(el.getAttribute("data-original"));
        // Only rescale amounts we could parse; leave "to taste" etc. untouched
        if (parsed !== null) {
            el.textContent = formatQuantity(parsed * ratio);
        }
    });

    scalerValue.textContent = currentServings;
    updateScalerButtons();
}

// Change servings by delta (clamped), then rescale
function changeServings(delta) {
    // Add delta but keep the result within [minServings, maxServings]
    currentServings = Math.min(maxServings, Math.max(minServings, currentServings + delta));
    scaleIngredients();
}

if (scalerMinus) scalerMinus.addEventListener("click", function () { changeServings(-1); });
if (scalerPlus) scalerPlus.addEventListener("click", function () { changeServings(1); });


/* ── Ingredient checkboxes ────────────────────────────────────── */

// Restore checked ingredients (stored by position) from sessionStorage
function loadCheckedState() {
    const saved = sessionStorage.getItem(STORAGE_KEY_CHECKS);
    if (!saved) return;

    // saved is a list of positions like [0, 2] — re-check those ingredients
    const checked = new Set(JSON.parse(saved));
    ingredientList.querySelectorAll(".ingredient-item").forEach(function (item, index) {
        if (checked.has(index)) item.classList.add("checked");
    });
}

// Save the indexes of the currently checked ingredients
function saveCheckedState() {
    // Record the position of every checked ingredient
    const checked = [];
    ingredientList.querySelectorAll(".ingredient-item").forEach(function (item, index) {
        if (item.classList.contains("checked")) checked.push(index);
    });
    sessionStorage.setItem(STORAGE_KEY_CHECKS, JSON.stringify(checked));
}

if (ingredientList) {
    ingredientList.addEventListener("click", function (e) {
        // Toggle the clicked ingredient and remember the new state
        const item = e.target.closest(".ingredient-item");
        if (!item) return;
        item.classList.toggle("checked");
        saveCheckedState();
    });
}

if (clearBtn) {
    clearBtn.addEventListener("click", function () {
        // Uncheck everything and forget the saved state
        ingredientList.querySelectorAll(".ingredient-item.checked")
            .forEach(function (item) { item.classList.remove("checked"); });
        sessionStorage.removeItem(STORAGE_KEY_CHECKS);
    });
}


/* ── Initialise ───────────────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {
    updateScalerButtons();   // set the initial +/- enabled state
    loadCheckedState();      // restore any ingredients ticked earlier
});
