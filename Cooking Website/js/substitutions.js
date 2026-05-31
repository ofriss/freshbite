// substitutions.js — live search filter for the ingredient substitutions table

// Filters table rows as the user types; shows an empty-state message when no rows match
document.getElementById('sub-search').addEventListener('input', function () {
    var term = this.value.trim().toLowerCase();
    var rows = document.querySelectorAll('#sub-table tbody tr');
    var visibleCount = 0;

    rows.forEach(function (row) {
        // Search across all cell text in the row
        var text = row.textContent.toLowerCase();
        var matches = !term || text.includes(term);
        row.style.display = matches ? '' : 'none';
        if (matches) visibleCount++;
    });

    // Show "no results" message only when every row is hidden
    document.getElementById('sub-empty').style.display = visibleCount === 0 ? '' : 'none';
});
