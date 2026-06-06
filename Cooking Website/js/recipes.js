// recipes.js — filter the recipe grid, badge "For You" matches, and remember
// filters when navigating to a recipe and back. Profile data comes from hidden
// fields whose IDs are passed in via RecipesConfig.

/* ── Elements ─────────────────────────────────────────────────── */

const cards = document.querySelectorAll(".recipe-card");
const emptyState = document.getElementById("recipes-empty");
const filterButtons = document.querySelectorAll(".filter-btn");


/* ── Logged-in user's profile ─────────────────────────────────── */

function fieldValue(id) {
    // Look up the hidden field by its server-generated id
    const el = document.getElementById(id);
    // Return its value, or "" if the field isn't on the page (guest)
    return el ? el.value : "";
}

const userDifficulty = fieldValue(RecipesConfig.hiddenDifficultyId);
const userCuisines = fieldValue(RecipesConfig.hiddenCuisinesId).split(",").filter(Boolean);
const isLoggedIn = userDifficulty !== "" || userCuisines.length > 0;

const rank = { Easy: 1, Medium: 2, Hard: 3 };


/* ── Filter state ─────────────────────────────────────────────── */

// "All" means that filter is off. One value per group — clicking replaces it.
const filters = { difficulty: "All", category: "All", cuisine: "All" };

// A card passes one filter if that filter is "All" or its data value matches.
function matches(type, card) {
    // "All" means the filter is off, so every card passes.
    // Otherwise the card's data-<type> value must equal the chosen value.
    return filters[type] === "All" || card.dataset[type] === filters[type];
}

// Show only cards that pass every filter.
function applyFilters() {
    let visible = 0;   // how many cards survived the filters

    cards.forEach(function (card) {
        // A card shows only if it passes all three filters at once
        const show = matches("difficulty", card)
            && matches("category", card)
            && matches("cuisine", card);

        // Add/remove the .hidden class to show or hide the card
        card.classList.toggle("hidden", !show);
        if (show) visible++;
    });

    // Show the "no results" message only when nothing is visible
    emptyState.hidden = visible > 0;
    // Remember the choices so the recipe page can restore them
    saveFilters();
}

// Highlight the chosen button in each group.
function highlightActive() {
    filterButtons.forEach(function (btn) {
        // This button is "active" when its value is the chosen one for its group
        const selected = filters[btn.dataset.filterType] === btn.dataset.filter;
        btn.classList.toggle("active", selected);
    });
}

filterButtons.forEach(function (btn) {
    btn.addEventListener("click", function () {
        // Record this button's value as the choice for its group (difficulty/category/cuisine)
        filters[btn.dataset.filterType] = btn.dataset.filter;
        // Update the button highlighting, then re-filter the grid
        highlightActive();
        applyFilters();
    });
});


/* ── "For You" badges ─────────────────────────────────────────── */

// Badge cards that suit the user's skill level and favourite cuisines.
function addForYouBadges() {
    // Guests have no profile, so there's nothing to recommend
    if (!isLoggedIn) return;

    cards.forEach(function (card) {
        // Suitable if the recipe is at or below the user's skill level
        const okDifficulty = !userDifficulty
            || (rank[card.dataset.difficulty] || 0) <= (rank[userDifficulty] || 0);
        // Suitable if the user has no cuisine preference, or this cuisine is one they like
        const okCuisine = userCuisines.length === 0
            || userCuisines.includes(card.dataset.cuisine);

        if (okDifficulty && okCuisine) {
            // Build the badge and drop it in at the start of the card's meta row
            const badge = document.createElement("span");
            badge.className = "for-you-badge";
            badge.textContent = "For You";
            card.querySelector(".card-meta").prepend(badge);
        }
    });
}


/* ── Guest banner dismiss ─────────────────────────────────────── */

const banner = document.querySelector(".guest-banner");
const bannerClose = document.getElementById("guest-banner-close");
if (banner && bannerClose) {
    bannerClose.addEventListener("click", function () {
        banner.style.display = "none";
    });
}


/* ── Filter persistence (so Recipe.aspx's Back button can restore) ── */

function saveFilters() {
    // Build a query string holding only the filters that are switched on
    const params = new URLSearchParams();
    Object.keys(filters).forEach(function (type) {
        // Skip "All" — it's the default and doesn't need saving
        if (filters[type] !== "All") params.set(type, filters[type]);
    });
    // Stash it so Recipe.aspx's Back button can return here with the same filters
    sessionStorage.setItem("recipes_filters", params.toString());
}


/* ── Initialise ───────────────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {
    // Read any filters passed in the URL (set by the recipe page's Back button)
    const params = new URLSearchParams(window.location.search);
    Object.keys(filters).forEach(function (type) {
        // Apply each one we find; missing ones keep their "All" default
        if (params.get(type)) filters[type] = params.get(type);
    });

    highlightActive();   // light up the restored buttons
    applyFilters();      // filter the grid to match
    addForYouBadges();   // tag recommended recipes for logged-in users
});
