/* ── 1. REFERENCES ───────────────────────────────────────────── */

const backBtn = document.getElementById("back-btn");
const scalerMinus = document.getElementById("scaler-minus");
const scalerPlus = document.getElementById("scaler-plus");
const scalerValue = document.getElementById("scaler-value");
const ingredientList = document.getElementById("ingredients-list");
const clearBtn = document.getElementById("clear-checks-btn");

const hiddenDifficulty = document.getElementById(RecipeConfig.hiddenDifficultyId);
const hiddenCuisines = document.getElementById(RecipeConfig.hiddenCuisinesId);

// Recipe ID from the URL — used as the sessionStorage key for checked ingredients
const recipeId = new URLSearchParams(window.location.search).get("id");

const STORAGE_KEY_CHECKS = "recipe_checks_" + recipeId;
const STORAGE_KEY_FILTERS = "recipes_filters";


/* ── 2. BACK BUTTON — preserve filters ───────────────────────── */

if (backBtn) {
    backBtn.addEventListener("click", function (e) {
        e.preventDefault();

        var saved = sessionStorage.getItem(STORAGE_KEY_FILTERS);
        if (saved) {
            // Filters were saved — go back with them as query params
            window.location.href = "/Recipes.aspx?" + saved;
        } else {
            window.location.href = "/Recipes.aspx";
        }
    });
}


/* ── 3. QUANTITY PARSER ──────────────────────────────────────── */

function parseQuantity(str) {
    if (!str) return null;

    var trimmed = str.trim();

    // Plain integer or decimal
    if (/^\d+(\.\d+)?$/.test(trimmed)) {
        return parseFloat(trimmed);
    }

    // Simple fraction e.g. "1/2"
    if (/^\d+\/\d+$/.test(trimmed)) {
        var parts = trimmed.split("/");
        return parseInt(parts[0]) / parseInt(parts[1]);
    }

    // Mixed number e.g. "1 1/2"
    if (/^\d+\s+\d+\/\d+$/.test(trimmed)) {
        var spaceIdx = trimmed.indexOf(" ");
        var whole = parseInt(trimmed.substring(0, spaceIdx));
        var fracParts = trimmed.substring(spaceIdx + 1).split("/");
        return whole + (parseInt(fracParts[0]) / parseInt(fracParts[1]));
    }

    // Not a recognised numeric format — unscalable
    return null;
}


/* ── 4. QUANTITY FORMATTER ───────────────────────────────────── */

function formatQuantity(num) {
    if (num === Math.floor(num)) {
        // Whole number
        return String(num);
    }

    // Try to express as a simple fraction or mixed number
    var fractions = [
        [1, 8], [1, 4], [1, 3], [3, 8],
        [1, 2], [5, 8], [2, 3], [3, 4], [7, 8]
    ];

    var whole = Math.floor(num);
    var remainder = num - whole;

    for (var i = 0; i < fractions.length; i++) {
        var n = fractions[i][0];
        var d = fractions[i][1];

        if (Math.abs(remainder - n / d) < 0.01) {
            var fracStr = n + "/" + d;
            return whole > 0 ? whole + " " + fracStr : fracStr;
        }
    }

    // Fallback — round to 2 decimal places
    return parseFloat(num.toFixed(2)).toString();
}


/* ── 5. SERVINGS SCALER ──────────────────────────────────────── */

var currentServings = RecipeConfig.defaultServings;
var minServings = 1;
var maxServings = 100;

function updateScalerButtons() {
    scalerMinus.disabled = currentServings <= minServings;
    scalerPlus.disabled = currentServings >= maxServings;
}

function scaleIngredients() {
    var ratio = currentServings / RecipeConfig.defaultServings;

    document.querySelectorAll(".ingredient-quantity").forEach(function (el) {
        var original = el.getAttribute("data-original");
        var parsed = parseQuantity(original);

        if (parsed !== null) {
            // Scalable quantity — multiply by ratio and format
            el.textContent = formatQuantity(parsed * ratio);
        }
        // Unscalable — leave untouched (el.textContent stays as original)
    });

    scalerValue.textContent = currentServings;
    updateScalerButtons();
}

if (scalerMinus) {
    scalerMinus.addEventListener("click", function () {
        if (currentServings > minServings) {
            currentServings--;
            scaleIngredients();
        }
    });
}

if (scalerPlus) {
    scalerPlus.addEventListener("click", function () {
        if (currentServings < maxServings) {
            currentServings++;
            scaleIngredients();
        }
    });
}


/* ── 6. INGREDIENT CHECKBOXES ────────────────────────────────── */

function loadCheckedState() {
    var saved = sessionStorage.getItem(STORAGE_KEY_CHECKS);
    if (!saved) return;

    var checkedIndexes = JSON.parse(saved);
    var items = ingredientList.querySelectorAll(".ingredient-item");

    items.forEach(function (item, index) {
        if (checkedIndexes.indexOf(index) !== -1) {
            item.classList.add("checked");
        }
    });
}

function saveCheckedState() {
    var items = ingredientList.querySelectorAll(".ingredient-item");
    var checked = [];

    items.forEach(function (item, index) {
        if (item.classList.contains("checked")) {
            checked.push(index);
        }
    });

    sessionStorage.setItem(STORAGE_KEY_CHECKS, JSON.stringify(checked));
}

if (ingredientList) {
    ingredientList.addEventListener("click", function (e) {
        var item = e.target.closest(".ingredient-item");
        if (!item) return;

        item.classList.toggle("checked");
        saveCheckedState();
    });
}

if (clearBtn) {
    clearBtn.addEventListener("click", function () {
        ingredientList.querySelectorAll(".ingredient-item.checked")
            .forEach(function (item) { item.classList.remove("checked"); });
        sessionStorage.removeItem(STORAGE_KEY_CHECKS);
    });
}


/* ── 7. INITIALISE ───────────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {
    updateScalerButtons();
    loadCheckedState();
});