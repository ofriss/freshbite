// recipes.js — client-side filtering, "For You" badges, and filter state persistence
// for the Recipes listing page. User profile data is injected via hidden fields.

/* ── 1. REFERENCES ───────────────────────────────────────────── */

const cards = document.querySelectorAll(".recipe-card");
const emptyState = document.getElementById("recipes-empty");
const guestBanner = document.querySelector(".guest-banner");
const guestClose = document.getElementById("guest-banner-close");

const hiddenDifficulty = document.getElementById(RecipesConfig.hiddenDifficultyId);
const hiddenCuisines = document.getElementById(RecipesConfig.hiddenCuisinesId);


/* ── 2. USER PROFILE ─────────────────────────────────────────── */

// Read once — never changes during the session
const userDifficulty = hiddenDifficulty && hiddenDifficulty.value
    ? hiddenDifficulty.value
    : null;

const userCuisines = hiddenCuisines && hiddenCuisines.value
    ? new Set(hiddenCuisines.value.split(",").map(function (s) { return s.trim(); }))
    : new Set();

const isLoggedIn = userDifficulty !== null || userCuisines.size > 0;


/* ── 3. FILTER STATE ─────────────────────────────────────────── */

let activeDifficulty = "All";
let activeCategory = "All";
let activeCuisines = new Set();   // empty = All


/* ── 4. FOR YOU BADGES ───────────────────────────────────────── */

const difficultyRank = { "Easy": 1, "Medium": 2, "Hard": 3 };

// Stamps "For You" badges on cards matching the logged-in user's skill and cuisine preferences
function applyForYouBadges() {
    if (!isLoggedIn) return;

    const userRank = difficultyRank[userDifficulty] || 0;

    cards.forEach(function (card) {
        const cardDifficulty = card.getAttribute("data-difficulty");
        const cardRank = difficultyRank[cardDifficulty] || 0;

        const matchesDifficulty = !userDifficulty
            || cardRank <= userRank;

        const matchesCuisine = userCuisines.size === 0
            || userCuisines.has(card.getAttribute("data-cuisine"));

        if (matchesDifficulty && matchesCuisine) {
            const meta = card.querySelector(".card-meta");
            if (meta && !meta.querySelector(".for-you-badge")) {
                const badge = document.createElement("span");
                badge.className = "for-you-badge";
                badge.textContent = "For You";
                meta.insertBefore(badge, meta.firstChild);
            }
        }
    });
}


/* ── 5. CORE FILTER FUNCTION ─────────────────────────────────── */

// Shows/hides recipe cards based on all three active filter dimensions
function applyFilters() {
    let visibleCount = 0;

    cards.forEach(function (card) {
        const passDifficulty = activeDifficulty === "All"
            || card.getAttribute("data-difficulty") === activeDifficulty;

        const passCategory = activeCategory === "All"
            || card.getAttribute("data-category") === activeCategory;

        const passCuisine = activeCuisines.size === 0
            || activeCuisines.has(card.getAttribute("data-cuisine"));

        const visible = passDifficulty && passCategory && passCuisine;
        card.classList.toggle("hidden", !visible);
        if (visible) visibleCount++;
    });

    emptyState.hidden = visibleCount > 0;
}


/* ── 6. SINGLE-SELECT HANDLER (Difficulty + Category) ───────── */

// Wires up mutually-exclusive filter buttons (difficulty and category)
function bindSingleSelect(filterType, onSelect) {
    const btns = document.querySelectorAll("[data-filter-type='" + filterType + "']");

    btns.forEach(function (btn) {
        btn.addEventListener("click", function () {
            btns.forEach(function (b) { b.classList.remove("active"); });
            btn.classList.add("active");
            onSelect(btn.getAttribute("data-filter"));
            applyFilters();
        });
    });
}

bindSingleSelect("difficulty", function (val) { activeDifficulty = val; });
bindSingleSelect("category", function (val) { activeCategory = val; });


/* ── 7. MULTI-SELECT HANDLER (Cuisine) ───────────────────────── */

const cuisineBtns = document.querySelectorAll("[data-filter-type='cuisine']");

// Keeps cuisine button active states in sync with the activeCuisines Set
function syncCuisineButtons() {
    cuisineBtns.forEach(function (btn) {
        const val = btn.getAttribute("data-filter");
        btn.classList.toggle("active",
            val === "All" ? activeCuisines.size === 0 : activeCuisines.has(val)
        );
    });
}

cuisineBtns.forEach(function (btn) {
    btn.addEventListener("click", function () {
        const val = btn.getAttribute("data-filter");

        if (val === "All") {
            activeCuisines.clear();
        } else {
            if (activeCuisines.has(val)) {
                activeCuisines.delete(val);
            } else {
                activeCuisines.add(val);
            }
        }

        syncCuisineButtons();
        applyFilters();
    });
});


/* ── 8. INITIALISE ───────────────────────────────────────────── */

document.addEventListener("DOMContentLoaded", function () {

    // Set All as active on every filter group by default
    document.querySelectorAll("[data-filter='All']")
        .forEach(function (btn) { btn.classList.add("active"); });

    // Stamp For You badges once — profile never changes during the session
    applyForYouBadges();
});


/* ── 9. GUEST BANNER DISMISS ─────────────────────────────────── */

if (guestClose && guestBanner) {
    guestClose.addEventListener("click", function () {
        guestBanner.style.display = "none";
    });
}

/* ── 10. FILTER STATE PERSISTENCE ───────────────────────────── */

// Serialises current filter state to sessionStorage so Recipe.aspx can restore it on back-navigate
function saveFilterState() {
    var params = new URLSearchParams();

    if (activeDifficulty !== "All") params.set("difficulty", activeDifficulty);
    if (activeCategory !== "All") params.set("category", activeCategory);
    if (activeCuisines.size > 0) params.set("cuisines", Array.from(activeCuisines).join(","));

    sessionStorage.setItem("recipes_filters", params.toString());
}

// Hook into existing filter handlers by wrapping applyFilters
var _originalApplyFilters = applyFilters;
applyFilters = function () {
    _originalApplyFilters();
    saveFilterState();
};

// Restore filter state if arriving from a recipe page
document.addEventListener("DOMContentLoaded", function () {
    var params = new URLSearchParams(window.location.search);

    var difficulty = params.get("difficulty");
    var category = params.get("category");
    var cuisines = params.get("cuisines");

    if (difficulty) {
        activeDifficulty = difficulty;
        document.querySelectorAll("[data-filter-type='difficulty']")
            .forEach(function (b) {
                b.classList.toggle("active", b.getAttribute("data-filter") === difficulty);
            });
    }

    if (category) {
        activeCategory = category;
        document.querySelectorAll("[data-filter-type='category']")
            .forEach(function (b) {
                b.classList.toggle("active", b.getAttribute("data-filter") === category);
            });
    }

    if (cuisines) {
        activeCuisines = new Set(cuisines.split(","));
        syncCuisineButtons();
    }

    // Re-run filters and badges with restored state
    if (difficulty || category || cuisines) {
        applyFilters();
        applyForYouBadges();
    }
});